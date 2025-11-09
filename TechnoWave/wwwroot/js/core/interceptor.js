(function () {

    const originalFetch = window.fetch;

    window.fetch = async function (url, init = {}) {

        init.headers = init.headers || {};

        const token = localStorage.getItem("jwtToken");
        if (token) {
            init.headers["Authorization"] = "Bearer " + token;
        }

        return originalFetch(url, init);
    };

})();
