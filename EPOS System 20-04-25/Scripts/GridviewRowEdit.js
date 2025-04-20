    $(document).ready(function () {
        $(".updatebutton").click(function () {
            var row = $(this).closest("tr");
            //Getting the ID of the record and the data from textbox to update
            var pID = $(this).attr("id");
            var update_data = $(".stock", row).val(); //stock is the CssClass name of the textbox.
            // If you are updating  more than one column, retrieve the value of all the
            //textboxes similarly and then add them in single variable
            //(see below)
            update_data = pID + "," + update_data; //adding both the value in single variable
            //with ',' separator                                
            // Confirming the operation from the user
            if (confirm("Do you want to update this record?")) {
                $.ajax({
                    type: "POST",
                    //UpdateRecordInGridViewUsingJQuery.aspx is the page name and UpdateProduct 
                    // is the server side web method which actually does the updation
                    url: "UpdateRecordInGridViewUsingJQuery.aspx/UpdateProduct",
                    //Passing the record id and data to be updated which is in the variable record_id
                    data: "{'args': '" + update_data + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //Giving message to user on successful updation
                    success: function () {
                        alert("Record successfully updated!!!");
                        //location.reload();
                    }
                });
            }
            return false;
        });
    });
