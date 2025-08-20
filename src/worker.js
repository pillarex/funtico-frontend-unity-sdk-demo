const guessType = (p) => {
    const lc = p.toLowerCase();
    if (lc.endsWith(".html")) return "text/html; charset=utf-8";
    if (lc.endsWith(".js")) return "application/javascript; charset=utf-8";
    if (lc.endsWith(".css")) return "text/css; charset=utf-8";
    if (lc.endsWith(".json")) return "application/json; charset=utf-8";
    if (lc.endsWith(".wasm")) return "application/wasm";
    if (lc.endsWith(".png")) return "image/png";
    if (lc.endsWith(".jpg") || lc.endsWith(".jpeg")) return "image/jpeg";
    if (lc.endsWith(".svg")) return "image/svg+xml";
    if (lc.endsWith(".ico")) return "image/x-icon";
    if (lc.endsWith(".pck")) return "application/octet-stream";
    return "application/octet-stream";
};

const isHtml = (k) => k.endsWith(".html");

const addCommonHeaders = (h) => {
    h.set("Cross-Origin-Opener-Policy", "same-origin");
    h.set("Cross-Origin-Embedder-Policy", "credentialless");
    h.set("X-Content-Type-Options", "nosniff");
    h.set("Access-Control-Allow-Origin", "*");
    h.set("Access-Control-Allow-Methods", "GET, HEAD, OPTIONS");
    h.set("Access-Control-Allow-Headers", "*");
    h.set("Access-Control-Expose-Headers", "ETag, Content-Length");
    h.set("Cross-Origin-Resource-Policy", "cross-origin");
    h.set("Timing-Allow-Origin", "*");
    return h;
};

export default {
    async fetch(request, env, ctx) {
        if (request.method === "OPTIONS") {
            const headers = addCommonHeaders(new Headers());
            const reqHeaders = request.headers.get("Access-Control-Request-Headers");
            headers.set("Access-Control-Allow-Headers", reqHeaders || "*");
            headers.set("Access-Control-Max-Age", "86400");
            return new Response(null, { status: 204, headers });
        }
        if (request.method !== "GET" && request.method !== "HEAD") {
            return new Response("Method Not Allowed", { status: 405 });
        }

        const url = new URL(request.url);
        let key = decodeURIComponent(url.pathname.replace(/^\/+/, ""));
        if (!key || key.endsWith("/")) key += "index.html";
        if (!key.includes(".") && key !== "index.html") key += "/index.html";

        // Try cache first
        const cache = caches.default;
        const cacheKey = new Request(url.toString(), { headers: request.headers, method: "GET" });
        let resp = await cache.match(cacheKey);
        if (resp) {
            if (request.method === "HEAD") {
                return new Response(null, { status: resp.status, headers: resp.headers });
            }
            return resp;
        }

        // Fetch from R2
        let obj = await env.BUCKET.get(key);
        if (!obj && key !== "index.html") {
            // Fallback to index.html for SPA-like routing
            obj = await env.BUCKET.get("index.html");
            if (!obj) return new Response("Not Found", { status: 404 });
            key = "index.html";
        } else if (!obj) {
            return new Response("Not Found", { status: 404 });
        }

        const headers = addCommonHeaders(new Headers());
        const contentType = obj.httpMetadata?.contentType || guessType(key);
        headers.set("Content-Type", contentType);
        if (obj.httpEtag) headers.set("ETag", obj.httpEtag);

        // Cache policy: HTML is short-lived, everything else long-lived
        if (isHtml(key)) {
            headers.set("Cache-Control", "public, max-age=60, must-revalidate");
        } else {
            headers.set("Cache-Control", "public, max-age=31536000, immutable");
        }

        // HEAD support
        if (request.method === "HEAD") {
            return new Response(null, { status: 200, headers });
        }

        resp = new Response(obj.body, { status: 200, headers });

        // Put in edge cache (respects Cache-Control)
        ctx.waitUntil(cache.put(cacheKey, resp.clone()));

        return resp;
    },
};
