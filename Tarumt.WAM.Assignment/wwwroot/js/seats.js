function select_seat(seat_num) {
  var seat = document.getElementsByClassName("seat")[seat_num - 1];

  if (seat.classList.contains("sold")) {
    available_seat.push(seat_num);
    seat.classList.remove("sold");
  } else {
    available_seat.splice(available_seat.indexOf(seat_num), 1);
    seat.classList.add("sold");
  }

  available_seat.sort(function (a, b) {
    return a - b;
  });
}
