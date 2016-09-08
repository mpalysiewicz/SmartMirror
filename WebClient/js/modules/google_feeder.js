function GoogleFeeder (callback) {
    this.callback = callback;
}

GoogleFeeder.prototype.init = function() {
    var feeder = this;
    gapi.load("client", function() { feeder.authenticate() });
}

GoogleFeeder.prototype.getEvents = function() {
    var feeder = this;

    var request = gapi.client.calendar.events.list({
        'calendarId': 'primary',
        'timeMin': (new Date()).toISOString(),
        'showDeleted': false,
        'singleEvents': true,
        'maxResults': 10,
        'orderBy': 'startTime'
    });

    request.execute(function(resp) {
        var events = feeder.mapEvents(resp.items);
        feeder.callback(events);
    });
}

GoogleFeeder.prototype.mapEvents = function(source) {
    var events = [];
    for (i = 0; i < source.length && i < 5; i++) {
        var event = {};

        event.title = source[i].summary;
        event.location = source[i].summary;
        event.startTime = source[i].start.dateTime;
        if (!event.startTime) { event.startTime = source[i].start.date; }
        event.endTime = source[i].end.dateTime;
        if (!event.endTime) { event.endTime = source[i].start.date; }

        events.push(event);
    }
    return events;
}

GoogleFeeder.prototype.authenticate = function() {    
    var feeder = this;
    gapi.auth.authorize({
        'client_id': config.calendarGoogle.clientId,
        'scope': config.calendarGoogle.scopes.join(' '),
        'immediate': true
    }, function (authResult) { feeder.authenticationResult(authResult) });
}

GoogleFeeder.prototype.authenticationResult = function(authResult) {
    var feeder = this;
    var authorizeDiv = document.getElementById('authorize-div');
    if (authResult && !authResult.error) {
        authorizeDiv.style.display = 'none';
        gapi.client.load('calendar', 'v3', function() { feeder.getEvents() });
    } else {
        authorizeDiv.style.display = 'inline';
    }
}