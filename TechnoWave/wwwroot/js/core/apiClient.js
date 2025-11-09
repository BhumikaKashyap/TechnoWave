window.apiClient = {

    async request(method, url, data = null) {

        const options = {
            method: method,
            headers: {
                "Content-Type": "application/json"
            }
        };

        if (data) {
            options.body = JSON.stringify(data);
        }

        // ✅ Call API
        const response = await fetch(`${appConfig.apiBaseUrl}${url}`, options);

        let result;
        try {
            result = await response.json();
        } catch {
            toastr.error("Invalid API response.");
            throw new Error("Invalid JSON");
        }

        // ✅ Global error handler
        if (!result.isSuccess) {
            toastr.error(result.message ?? "Something went wrong");
            throw result;
        }

        toastr.success(result.message);
        return result.data;
    },

    // ✅ Shortcut methods:
    get(url) {
        return this.request("GET", url);
    },

    post(url, data) {
        return this.request("POST", url, data);
    },

    put(url, data) {
        return this.request("PUT", url, data);
    },

    delete(url) {
        return this.request("DELETE", url);
    }
};
