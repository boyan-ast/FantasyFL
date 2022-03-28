document.querySelector('body')
    .addEventListener("onload", addRowHandlers());

function addRowHandlers() {
    let table = document.getElementById("playersTable");

    let rows = table.getElementsByTagName("tr");

    for (i = 0; i < rows.length; i++) {
        let currentRow = table.rows[i];

        let createClickHandler = function (row) {
            return function (e) {
                let radioElement = e.currentTarget.querySelector('input[type="radio"]');
                if (radioElement != null) {
                    radioElement.checked = true;

                    let removePlayerInputElement = document.getElementById("action-btn");

                    if (removePlayerInputElement != null) {
                        removePlayerInputElement.removeAttribute("hidden");
                    }
                }
            };
        };

        currentRow.onclick = createClickHandler(currentRow);
    }
}
