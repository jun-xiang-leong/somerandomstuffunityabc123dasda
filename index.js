const dgram = require('dgram');
const fs = require('fs');
const client = dgram.createSocket('udp4');

// Read the JSON file
class JsonPackage{
    constructor(code){
        this.UID = 123456;
        this.lon = 103.8119;
        this.lat = 1.3838;
        this.symbolCode = code;
        this.description = "hospital";
        this.additionalInfomation = "field hospital";
    }
}
let packages =[];
let pack = new JsonPackage("SFGPIXH---H----");
packages.push(pack);

pack.symbolCode = "SUGPIXH---H----";
pack.lon += 0.01;
packages.push(pack);

pack.symbolCode = "SHGPIXH---H----";
pack.lon += 0.01;
packages.push(pack);

pack.symbolCode = "SFGPUCFRMS-----";
pack.lon += 0.01;
packages.push(pack);

pack.symbolCode = "SUGPUCFRMS-----";
pack.lon += 0.01;
packages.push(pack);

pack.symbolCode = "SHGPUCFRMS-----";
pack.lon += 0.01;
packages.push(pack);

// Send the JSON data over UDP
const serverAddress = '127.0.0.1'; // IP of the receiver
const serverPort = 12345; // Port on the receiver
array.forEach(element => {
    const message = JSON.stringify(pack);
    client.send(message, 0, message.length, serverPort, serverAddress, (err) => {
    if (err) {
        console.error('Error sending data:', err);
        } else {
            console.log('Sent JSON data over UDP');
        }
    });
});
client.close();
