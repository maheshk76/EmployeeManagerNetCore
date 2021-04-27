$(document).ready(function () {
    $("#Sdate").on("change", function () {
        $("#Edate").attr("min", $(this).val());
    });
    $(".search-btn").on("click", function () {
        LoadData($(this));
    });

    $("#Search").autocomplete({
        select: function (e) {
            LoadData();
        },
        
       /* source: '/api/product/search'*/
        source: function (request, response) {
            $.ajax({

                url: '/api/search',
                data: {
                    term: request.term
                }
            })
                .done(function (data) {
                    console.log(data);
                    var array = $.map(data, function (m) {
                        kvs = m.split("|");
                        return {
                            label: m,
                            value :kvs[0]
                        };
                    });
                    response(array);
                    var sug = $(".ui-menu-item > div[id^='ui-id-']");
                    console.log(typeof sug);
                    sug.each(function(k, v) {
                        console.log(k + " " + v.innerHTML);
                        var kv = v.innerHTML.split("|");
                        if (kv[1].includes("Project")) {
                            var t = "<b class='alert-info text-info' style='float:right;font-size:12px;border-radius:20px;padding:2px 14px 2px;'>" + kv[1] + "</b>";

                            v.innerHTML = kv[0] + t;
                        }
                        else {
                            v.innerHTML = kv[0];
                        }
                    });
                })
                .fail(function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("FAIL");
                });
        }
    });
    $("#Search").keyup(function () {
        if (!this.value) {
            LoadData();
    }
    });
    $("#Pagesize").on("change", function () {
        LoadData();
    });
    $(".clear-btn").on("click", function () {
        $("#Search").val('');
        $("#EmpId").val('');
        $("#CityId").val('');
        $("#Sdate").val('');
        $("#Edate").val('');
        $("#curpageidx").val('');
        LoadData();
    });

});
function LoadData() {
    $(".tp-loader").show();
    $.ajax({

        url: '/Home/MainList/',
        data: {
            search: $("#Search").val(),
            sdate: $("#Sdate").val(),
            edate: $("#Edate").val(),
            cPage: $("#curpageidx").val(),
            Pagesize: $("#Pagesize").val()
        }
    })
        .done(function (response) {
            $(".datadiv").html(response);
            $(".tp-loader").hide();
        })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert("FAIL");
        })
        .always(function () {
        });
}
LoadData();

function Goto(index) {
    document.getElementById("curpageidx").value = index;
    LoadData();

}
$(document).ready(function () {
    
    $(".deleteproject").click("click",function () {
        var id = $(this).attr("data-value");
        bootbox.confirm("Are you sure want to delete?", function (result) {
            if (result) {
                $.ajax({

                    url: '/Project/Delete',
                    type:"POST",
                    data: {
                        id: id
                    }
                })
                    .done(function (response) {
                        toastr.success("deleted succesfully");
                        setTimeout(function () { window.location.reload(); }, 2000);
                    })
                    .fail(function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("FAIL");
                    })
                    .always(function () {
                    });
            }
        });
    });
   
    $(".deleteEmp").on("click",function () {
        var id = $(this).attr("data-value");
        bootbox.confirm("Are you sure want to delete?", function (result) {
            if (result) {
                $.ajax({

                    url: '/Employee/Delete',
                    type: "POST",
                    data: {
                        id: id
                    }
                })
                    .done(function (response) {
                        toastr.success("deleted succesfully");
                        setTimeout(function () { window.location.reload(); }, 2000);
                    })
                    .fail(function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("FAIL");
                    })
                    .always(function () {
                    });
            }
    });
    });

});