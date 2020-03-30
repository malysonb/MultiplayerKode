'use strict';
const WebSocket = require('ws');

var http = require('http');
var port = process.env.PORT || 80;

var nofclients = 0;

http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Server is online\n there is ' + nofclients + ' players online');
}).listen(port);

const wss = new WebSocket.Server({ port: 3000 });

wss.on('connection', function connection(ws) {
    console.log("Connected!");
    nofclients++;
    ws.on('message', function incoming(message) {
        console.log('received: %s', message);
        wss.clients.forEach(function each(client) {
            if (client.readyState === WebSocket.OPEN) {
                client.send(message);
            }
        });
    });

    ws.send('something');
});
