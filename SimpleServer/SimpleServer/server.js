var express = require('express');
var bodyParser = require('body-parser');

var app = express();
app.use(bodyParser.json());


app.post('/', function (request, response) {
    console.log(request.body);
});

app.get('/', function (req, res) {
    res.send('Yo');
});

//var http = require('http');
//var port = process.env.port || 3000;
//http.createServer(function (req, res) {
//    res.writeHead(200, { 'Content-Type': 'text/plain' });
//    res.end('Hello World\n');
//}).listen(port);
    app.listen(3000);