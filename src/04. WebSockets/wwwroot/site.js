listen = (id) => {
    const socket = new WebSocket(`wss://localhost:44308/Coffee/${id}`);

    socket.onmessage = event => {
        document.getElementById("status").innerHTML = JSON.parse(event.data);
    };

    // We can send data back to the server: socket.send();
};

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const product = document.getElementById("product").value;
    const size = document.getElementById("size").value;
    fetch("/Coffee",
        {
            method: "POST",
            body: { product, size }
        })
        .then(response => response.text())
        .then(id => {
            document.getElementById("status").innerHTML = `Starting coffee #${id}`;
            listen(id);
        });
});
