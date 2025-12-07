const input = document.getElementById('autocomplete-input');
input.addEventListener('input', function () {
    const userInput = input.value;

    if (userInput.length >= 3) {
        fetch(`/Invoice/GetVendorsList?VendorName=${userInput}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ input: userInput })
        })
            .then(response => response.json())
            .then(data => {
                data = JSON.parse(data);
                var a = document.createElement("div");
                a.setAttribute("class", "autocomplete-items");
                this.parentNode.appendChild(a);
                for (i = 0; i < data.length; i++) {
                    var b = document.createElement("div");
                    //b.innerHTML = "<strong>" + data[i].substr(0, userInput.length) + "</strong>";
                    b.innerHTML += data[i].substr(userInput.length);
                    b.innerHTML += "<input type='hidden' value='" + data[i] + "'>";
                    b.addEventListener("click", function (e) {
                        input.value = this.getElementsByTagName("input")[0].value;
                        closeAllLists();
                    });
                    a.appendChild(b);
                };
            })
            .catch(error => console.error('Error fetching autocomplete suggestions:', error));
    }
    else {
        input.innerHTML = '';
    }
});
document.addEventListener("click", function (e) {
    closeAllLists(e.target);
});
function closeAllLists(elmnt) {
    var x = document.getElementsByClassName("autocomplete-items");
    for (var i = 0; i < x.length; i++) {
        if (elmnt != x[i] && elmnt != input) {
            x[i].parentNode.removeChild(x[i]);
        }
    }
}