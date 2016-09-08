function OAuth2 (config) {
    this.config = config;
}

OAuth2.prototype.authenticate = function(callback) {
    var authCode = this.loadAuthCode();
    if(authCode == null)
        if(!this.acquireAuthCode())
            return;

    var token = this.loadToken();
    if(token == null) 
        this.getNewToken(callback);    
    else {
        if(token.expires_at < this.getCurrentSecods())
            this.renewToken(callback);
        else
            callback();
    }
}

OAuth2.prototype.loadAuthCode = function() {
    return JSON.parse(localStorage.getItem(this.config.providerId + '.authCode'));
}

OAuth2.prototype.saveAuthCode = function(authCode) {
    localStorage.setItem(this.config.providerId + '.authCode', JSON.stringify(authCode));
}

OAuth2.prototype.loadToken = function() {
    return JSON.parse(localStorage.getItem(this.config.providerId + '.token'));
}

OAuth2.prototype.saveToken = function(token) {
    localStorage.setItem(this.config.providerId + '.token', JSON.stringify(token));
}

OAuth2.prototype.acquireAuthCode = function( ) {
    var args = this.getArgs();
    
    if(args === undefined || args.code === undefined) {
        this.redirectToProvider();
        return false;
    }

    this.saveAuthCode(args.code);
    return true;
}

OAuth2.prototype.getNewToken = function(callback) {
    var args = {
        client_id: this.config.clientId,
        client_secret: this.config.clientSecret,
        grant_type: 'authorization_code',
        code:  this.loadAuthCode(),       
    };
    this.acquireToken(args, callback);
}

OAuth2.prototype.renewToken = function(callback) {
    var token = this.loadToken();
    var args = {
        client_id: this.config.clientId,
        client_secret: this.config.clientSecret,
        grant_type: 'refresh_token',
        refresh_token:  token.refresh_token,
    };
    this.acquireToken(args, callback);
}

OAuth2.prototype.acquireToken = function(args, callback) {
    var oauth = this;
    $.ajax({
        method: 'POST',
        url: this.config.token,
        data: this.createUrlParams(args)
    }).done(function(data){
        data.expires_at = oauth.getCurrentSecods() + data.expires_in
        oauth.saveToken(data);
        callback();
    }).fail(function(data, status){
        alert(JSON.stringify(data));
    })
}

OAuth2.prototype.getArgs = function() {
    url = window.location.href;
    query = url.split('?')[1];
    if (typeof query != 'undefined')
    {
        params = query.split('&');
        var args = {};
        for(i=0; i< params.length; i++)
        {
            keyValue = params[i].split('=');
            args[keyValue[0]] = keyValue[1];
        }
    }
    return args;
}

OAuth2.prototype.redirectToProvider = function() {
    var url = this.createUrlWithParams(this.config.authorization, {
            response_type: 'code',
            client_id: this.config.clientId,
            scope: this.config.scopes.join(' '),
        }
    );
    window.location.replace(url);
}

OAuth2.prototype.getHeaders = function() {
    var token = this.loadToken();
    return { Authorization: token.token_type + ' ' + token.access_token };
}

OAuth2.prototype.createUrlWithParams = function(url, object) {
    return url + '?' + this.createUrlParams(object);
}

OAuth2.prototype.createUrlParams = function(object) {
    var str = [];
    for(item in object) {
        str.push(item + '=' + object[item]);
    }
    return str.join('&');
}

OAuth2.prototype.getCurrentSecods = function() {
    return new Date().getTime() / 1000
}