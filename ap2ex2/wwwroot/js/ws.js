
var connection = new signalR.HubConnectionBuilder().withUrl("/MessageHub").build();
connection.start()

connection.on("MessageReceived", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    
    const rowDiv = document.createElement("div");
    const colDiv = document.createElement("div");
    const spanD = document.createElement("span");

    const messageText = document.createTextNode(msg);

    rowDiv.className = "row";
    colDiv.className = "col";
    spanD.className = "othersSpeechBubble";

    spanD.appendChild(messageText);
    colDiv.appendChild(spanD);
    rowDiv.appendChild(colDiv);
    document.getElementById("sentChat").appendChild(rowDiv);
});

document.getElementById("sendTextMessageButton").addEventListener("click", function (event) {
    var user = document.getElementById("userId").innerText;
    var receiverConnectionId = document.getElementById("userInChatId").innerText;
    var message = document.getElementById("message-to-send").value;
    connection.invoke("SendToUser", user, receiverConnectionId, message).catch(function (err) {
        return console.error(err.toString());
    });
    
    const rowDiv = document.createElement("div");
    const colDiv = document.createElement("div");
    const spanD = document.createElement("span");
    
    rowDiv.className = "row";
    colDiv.className = "col";
    spanD.className = "usersSpeechBubble";

    spanD.appendChild(message);
    colDiv.appendChild(spanD);
    rowDiv.appendChild(colDiv);
    document.getElementById("sentChat").appendChild(rowDiv);
    
    event.preventDefault();
});



