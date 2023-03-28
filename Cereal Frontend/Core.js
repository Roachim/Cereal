


//Method for Getting a picture from cereal
$("#getCerealPicture").on("click", function(e) {
    e.preventDefault();
    //const projectName = $("#projectName").val().trim();
    $.ajax({
        url : "https://localhost:7134/api/Cereals/Picture/" +$("#cerealId").val(),
        type: 'GET',
        datatype : JSON,
        success : function(data){
          
            $("#pictureImg").attr ("src", "data:image/png;base64, " + data.picture);     //show table
        },
        error: function(jqxhr, status, exception) {
            console.log('Exception:', exception);
            console.log(status);
            console.log(jqxhr.status);
            console.log(exception.message);
            console.log(console.warn(jqxhr.responseText));
        }
    });
});

