pollWithTimeout = (url, options, timeout = 30000) => {
    return Promise.race([
        fetch(url, options),
        new Promise((_, reject) =>
            setTimeout(() => reject(new Error('timeout')), timeout)
        )
    ]);
};

poll = (orderId) => {
    pollWithTimeout(`/Coffee/${orderId}`)
        .then(response => {
                if (response.status === 200) {
                    response.json().then(j => {
                        document.getElementById("status").innerHTML = j.update;
                        if (!j.finished)
                            poll(orderId);
                    });
                }
            }
        )
        .catch(response => poll(orderId));
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
            poll(id);
        });
});
