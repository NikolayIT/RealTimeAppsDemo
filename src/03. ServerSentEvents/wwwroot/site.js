listen = (id) => {
    var eventSource = new EventSource(`/Coffee/${id}`);
    eventSource.onmessage = (event) => {
        document.getElementById("status").innerHTML = event.data;
    };
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
