document.addEventListener("DOMContentLoaded", () => {
    const input = document.querySelector(".input");
    const chooseButton = document.querySelector(".choose_button");
    const showSection = document.querySelector(".show_section");
    const uploadButton = document.querySelector(".upload_button");

    const files = new Set();

    chooseButton.addEventListener("click", () => {
        input.click();
    });

    input.addEventListener("change", (event) => {
        addNewFiles(event);
    });

    uploadButton.addEventListener("click", (event) => {
        if (files.size == 0) {
            alert("You can't load non-existing files.");
            event.preventDefault();
            return;
        }

        const dataTransfer = new DataTransfer();
        files.forEach(file => dataTransfer.items.add(file));
        input.files = dataTransfer.files;
        files.clear();
    })

    const addNewFiles = (event) => {
        const targetFiles = event.target.files;
        const newFiles = Array.from(targetFiles);
        newFiles.forEach((file) => {
            if (!files.has(file)) {
                files.add(file);
                updateFiles(file);
            }
        });

        input.value = '';
    }

    // Creates new row in the list of the testing files and binds remove sign with remove event
    const updateFiles = (file) => {
        const listElement = document.createElement("div");
        listElement.classList.add("list");

        const name = document.createElement("div");
        name.classList.add("name");
        const fileNameElement = document.createElement("div");
        fileNameElement.classList.add("file_name");
        fileNameElement.innerHTML = file.name;
        name.appendChild(fileNameElement);

        const remove = document.createElement("div");
        remove.classList.add("remove");
        const removeSign = document.createElement("div");
        removeSign.classList.add("remove_sign");
        removeSign.innerHTML = "x";
        remove.appendChild(removeSign);

        listElement.appendChild(name);
        listElement.appendChild(remove);
        showSection.appendChild(listElement);

        removeSign.addEventListener("click", () => {
            files.delete(file);
            showSection.removeChild(listElement);
        });
    }
});
