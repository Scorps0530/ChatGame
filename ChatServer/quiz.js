var fs = require("fs");

line = []
fs.readFile('./quiz.txt', 'utf8', function(err, data){
    data.split('\r\n').forEach(element => {
        if(element == '') {

        }
        else {
            line.push(element)
        }
    });

    line.forEach(element => {
        console.log(element)
    });

})


