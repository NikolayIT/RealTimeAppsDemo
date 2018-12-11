listen = (id) => {
    var eventSource = new EventSource(`/Coffee/${id}`);
    eventSource.onmessage = (event) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = event.data;
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
        .then(text => listen(text));
});
