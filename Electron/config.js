var config = {

    // Lenguage for the mirror
    language : "pl-PL",
    
    // Keyword Spotting (Hotword Detection)
    speech : {
        keyword : "Smart mirror",
        model : "smart_mirror.pmdl", // The name of your model
        sensitivity : 0.5, // Keyword getting too many false positives or not detecting? Change this.
        continuous: false // After a keyword is detected keep listening until speech is not heard
    },
    layout: "main",
    greeting : ["Cześć, dobrze wyglądasz!", "Greetings, commander"], // An array of greetings to randomly choose from

    // Alternativly you can have greetings that appear based on the time of day
    
    greeting : {
       night: ["Bed?", "zZzzZz", "Time to sleep"],
       morning: ["Good Morning"],
       midday: ["Hey!", "Hello"],
       evening: ["Good evening"]
    },
    

    //use this only if you want to hardcode your geoposition (used for weather)
    /*
    geo_position: {
       latitude: 78.23423423,
       longitude: 13.123124142
    },
    */
    
    // forcast.io
    forecast : {
        key : "256a0659d793a35f29a1577723c08fb3", // Your forcast.io api key
        units : "auto" // See forcast.io documentation if you are getting the wrong units
    },
    // Philips Hue
    hue : {
        ip : "", // The IP address of your hue base
        uername : "", // The username used to control your hue
        groups : [{
            id : 0, // The group id 0 will change all the lights on the network
            name : "all"
        }, {
            id : 1,
            name : "bedroom"
        }, {
            id : 2,
            name : "kitchen"
        }]
    },
    // Calendar (An array of iCals)
    calendar: {
      icals : ["https://calendar.google.com/calendar/ical/palysiewicz.miroslaw%40gmail.com/private-133d3ae21bec632995dd7ae9a1dac2ba/basic.ics",
				"https://outlook.office365.com/owa/calendar/504bc292b0d24df1bfc64fafde9ff5bd@pl.abb.com/26b66fd1369d4393a5948ee5900b7c828192427608738068664/calendar.ics"], // Be sure to wrap your URLs in quotes
      maxResults: 9, // Number of calender events to display (Defaults is 9)
      maxDays: 365 // Number of days to display (Default is one year)
    },
    // Giphy
    giphy: {
      key : "dc6zaTOxFJmzC" // Your Gliphy API key
    },
    // YouTube
    youtube: {
      key : "AIzaSyDV8sL5mHavqzBkD5Kf0RUjIiu7iMufrUE" // Your YouTube API key
    },
    // SoundCloud
    soundcloud: {
        key : "" // Your SoundCloud API key
    },
    traffic: {
      key : "AvyWHuZPLaMFzAgcgnTRTPYEbkmCFMk_HEh1CBpCss8gHYHOutAt8T_72-Bqof-7", // Bing Maps API Key

      reload_interval : 5, // Number of minutes the information is refreshed
      // An array of tips that you would like to display travel time for
      trips : [{
        mode : "Driving", // Possibilities: Driving / Transit / Walking
        origin : "Krzywda 17H, Krakow, Poland", // Start of your trip. Human readable address.
        destination : "Wielopole 1, Krakow, Poland", // Destination of your trip. Human readable address.
        name : "work", // Name of your destination ex: "work"
        /*startTime: "",
        endTime: ""*/ // Optional starttime and endtime when the traffic information should be displayed on screen. The format can be either hh:mm or hh:mm am/pm
      }]
    },
    rss: {
      feeds : ["http://rss.majsterkowo.pl/posty"],  // RSS feeds list - e.g. ["rss1.com", "rss2.com"]
      refreshInterval : 120 // Number of minutes the information is refreshed
    }
};

// DO NOT REMOVE
if (typeof module !== 'undefined') {module.exports = config;}
