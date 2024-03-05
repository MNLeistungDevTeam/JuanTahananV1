const progressConnection = new signalR.HubConnectionBuilder()
    .withUrl("/uploaderHub")
    .build();


progressConnection.on("ReceiveProgress", function (progressData) {
    console.log(progressData);

});
progressConnection.on("UpdateSwalProgress", function (progressData) {
    updateProgress(progressData);

});

progressConnection.start()
    .then(() => console.log('SignalR connection established.'))
    .catch(err => console.error('SignalR connection error:', err));