$(function () {

    $("a.delete").click(function () {
        if (!confirm("Подтвердите удаление страницы.")) return false;
    });

    $("table#pages tbody").sortable({
        items: "tr:not(.home)",
        placeholder: "ui-state-highlight",
        update: function () {
            var ids = $("table#pages tbody").sortable("serialize");
            var url = "/Admin/Pages/ReorderPages";
            $.post(url, ids);
        }
    });
});
