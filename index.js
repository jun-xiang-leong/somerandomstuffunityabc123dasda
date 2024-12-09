const dgram = require('dgram');
const fs = require('fs');
const client = dgram.createSocket('udp4');

// Read the JSON file
class JsonPackage{
    constructor(){
        this.UID = 123456;
        this.lon = 103.8119;
        this.lat = 1.3838;
        this.symbolCode = "SFGPIXH---H----";
        this.description = "hospital";
        this.additionalInfomation = "field hospital";
    }
}
let pack = new JsonPackage();
// Send the JSON data over UDP
const message = JSON.stringify(pack);
const serverAddress = '127.0.0.1'; // IP of the receiver
const serverPort = 12345; // Port on the receiver

client.send(message, 0, message.length, serverPort, serverAddress, (err) => {
    if (err) {
        console.error('Error sending data:', err);
    } else {
        console.log('Sent JSON data over UDP');
    }
    client.close();
});
