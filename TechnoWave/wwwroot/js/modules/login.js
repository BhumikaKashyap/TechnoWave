$(document).ready(() => {
    $("#btnLogin").click(async function () {
        const payload = {
            email: $("#email").val(),
            password: $("#password").val()
        };

        try {
            const data = await apiClient.post("/api/Auth/Login", payload);

            localStorage.setItem("jwtToken", data.token);
            localStorage.setItem("userName", data.name);

            window.location.href = "/Dashboard/Index";

        } catch (err) {
            
        }

    });

});
