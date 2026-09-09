window.foodDialog = {
    open: function (id) {
        const dialog = document.getElementById(id);

        if (dialog) {
            dialog.showModal();
        }
    },

    close: function (id) {
        const dialog = document.getElementById(id);

        if (dialog) {
            dialog.close();
        }
    }
};