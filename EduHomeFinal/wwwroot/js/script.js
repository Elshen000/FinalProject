$(document).ready(function () {




   

    $(document).on('keyup', '#teachersearch', function () {

        $.ajax({
            url: "/Teacher/TeacherSearch/ ",
            type: "GET",
            data: {
                "keyword1": $("#teachersearch").val()
            },
            success: function (response) {
                if ($("#teachersearch").val().length > 0) {

                    $("#ourTeachers div").slice().remove()
                    $("#ourTeachers").append(response)

                }
                else {
                    $("#ourTeachers ").empty(),
                        $("#ourTeachers").append(response)
                   
                }
               

            }
        });              

    })

    $(document).on('keyup', '#eventsearch', function () {

        $.ajax({
            url: "/Event/EventSearch/ ",
            type: "GET",
            data: {
                "keyword2": $("#eventsearch").val()
            },
            success: function (response) {
                if ($("#eventsearch").val().length > 0) {

                    $("#searchevent div").slice().remove()
                    $("#searchevent").append(response)

                }
                else {
                    $("#searchevent").empty(),
                        $("#searchevent").append(response)

                }


            }
        });

    })

    $(document).on('keyup', '#coursesearch', function () {

        $.ajax({
            url: "/Course/CourseSearch/ ",
            type: "GET",
            data: {
                "keyword3": $("#coursesearch").val()
            },
            success: function (response) {
                if ($("#coursesearch").val().length > 0) {

                    $("#searchcourse div").slice().remove()
                    $("#searchcourse").append(response)

                }
                else {
                    $("#searchcourse").empty(),
                        $("#searchcourse").append(response)

                }


            }
        });

    })

    $(document).on('keyup', '#blogsearch', function () {

        $.ajax({
            url: "/Blog/BlogSearch/ ",
            type: "GET",
            data: {
                "keyword4": $("#blogsearch").val()
            },
            success: function (response) {
                if ($("#blogsearch").val().length > 0) {

                    $("#searchblog div").slice().remove()
                    $("#searchblog").append(response)

                }
                else {
                    $("#searchblog").empty(),
                        $("#searchblog").append(response)

                }


            }
        });

    })

    $(document).on('keyup', '#input-search', function () {
        if ($("#input-search").val().length > 0) {
            $.ajax({
                url: "/Home/GlobalSearch/ ",
                type: "Get",
                data: {
                    "keywordd": $("#input-search").val()
                },
                success: function (response) {
                    $("#search-form li").slice().remove()
                    $("#search-form").append(response)
                   

                }
            });
        }
        else {
            $("#search-form li").remove()

        }

    })




    $(document).on('click', '#mc-embedded-subscribe2', function () {
        $.ajax({
            url: "/Home/SubScribe/",
            type: "Get",
            data: {
                "email": $("#mce-EMAIL").val(),
            },
            success: function (res) {
                $("#Pnim").empty();
                $("#Pnim").append(res);

            }
        })
    })

     //$(document).on('click', '.reply-btn', function () {
     //   $.ajax({
     //       url: "/Contact/SendMessage/",
     //       type: "Get",
     //       data: {
     //           "message": $("#message").val(),
     //           "email": $("#email").val(),
     //           "subject": $("#subject").val(),
     //           "name": $("#name").val(),
     //       },
     //       success: function (res) {
     //           $("#Pnim").empty();
     //           $("#Pnim").append(res);
     //           if ($("#email").val() != null) {
     //               $("#message").empty();
     //               $("#email").empty();
     //               $("#subject").empty();
     //               $("#name").empty();
     //           };
     //       }
     //   })
     //})

    $(document).on('click', '.reply-btn', function () {
        $.ajax({
            url: "/Contact/SendMessage/",
            type: "Get",
            data: {
                "message": $("#message").val(),
                "email": $("#email").val(),
                "subject": $("#subject").val(),
                "name": $("#name").val(),
            },
            success: function (res) {
                $("#SendMessage").empty();
                $("#SendMessage").append(res);
                if ($("#email").val() != null) {
                    $("#message").empty();
                    $("#email").empty();
                    $("#subject").empty();
                    $("#name").empty();
                };
            }
        })
    })
})