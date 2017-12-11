(function () {
    var config = {
        authority: "http://localhost:5000",
        client_id: "spa",
        popup_redirect_uri: "http://localhost:3003/callback.html",
        response_type: "id_token token",
        scope: "openid profile api1",
        post_logout_redirect_uri: "http://localhost:3003/signoutcallback.html",
    };
    window.oidc = { userManager: new UserManager(config) };
})();