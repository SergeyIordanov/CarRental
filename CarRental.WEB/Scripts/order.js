function countTotalPrice() {
    var source = document.getElementById("source_price");
    var target = document.getElementById("total_price");
    var driver = document.getElementById("radio_1");

    var current = +source.innerHTML.replace(',', '.');
    var res = 0;
    
    var fromArr = document.getElementById("fromDate").value.replace(",", "").split(" ");
    var toArr = document.getElementById("toDate").value.replace(",", "").split(" ");
    fromArr[1] = convertMonth(fromArr[1]);
    toArr[1] = convertMonth(toArr[1]);
    var fromDate = new Date(fromArr[2] + " " + fromArr[1] + " " + fromArr[0]);
    var toDate = new Date(toArr[2] + " " + toArr[1] + " " + toArr[0]);
    var diff = ((toDate - fromDate) / 86400000).toFixed();

    res += current * diff;

    if (driver.checked) {
        res += (diff * 20);
    }

    res = Math.round(res * 100) / 100;

    if(res < 0)
        target.innerHTML = 0;
    else
        target.innerHTML = res.toString().replace(',', '');
}

function convertMonth(month) {
    switch (month) {
    case "January":
        return 1;
    case "February":
        return 2;
    case "March":
        return 3;
    case "April":
        return 4;
    case "May":
        return 5;
    case "June":
        return 6;
    case "July":
        return 7;
    case "August":
        return 8;
    case "September":
        return 9;
    case "October":
        return 10;
    case "November":
        return 11;
    case "December":
        return 11;
    default:
        return 0;
    }
}