"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, content) {
    var msg = content;
    var encodedMsg = user + " says: " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.querySelector("#messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.querySelector("#sendButton").addEventListener("click", function (event) {
    var user = document.querySelector("#userInput").value;
    var content = document.querySelector("#messageInput").value;
    var recipientId = document.querySelector("#recipient").value;
    var chatRoomId = document.querySelector("#group").value;
    connection.invoke("Send", user, content, recipientId, chatRoomId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});