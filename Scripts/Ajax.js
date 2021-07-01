$(document).ready(function () {

	// Filter list by type
	$("#Type").change(function () {
		var typeVal = $(this).val();
		var val = $("#search").val();
		var data = $(this).closest(".flex").data("url");
		var url = "/Editor/_" + data + "List";

		$.ajax({
			type: "GET",
			url: url,
			dataType: "text",
			data: { val: val, typeVal: typeVal },
			success: function (data) {
				$(".list").html(data);
			}
		})
	});

	$("#search").keyup(function () {
		var typeVal = $("#Type").val();
		var val = $(this).val();
		var data = $(this).closest(".flex").data("url");
		var url = "/Editor/_" + data + "List";

		$.ajax({
			type: "GET",
			url: url,
			dataType: "text",
			data: { val: val, typeVal: typeVal },
			success: function (data) {
				$(".list").html(data);
			}
		})
	});

	// HOME CONTROLLER FILTERS

	// Filter list by instrument
	var instrumentChange = function () {
		$("#InstrumentID").change(function () {
			var typeVal = $(this).val();
			//var data = $(this).closest("div").data("url");  
			var url = "/Home/_MaterialTypeContainer";

			$.ajax({
				type: "GET",
				url: url,
				dataType: "text",
				data: { instrumentID: typeVal },
				success: function (data) {
					$("#materialTypeContainer").html(data);
					typeChange();
				}
			})

			$.ajax({
				type: "GET",
				url: "/Home/_MaterialContainer",
				dataType: "text",
				data: { instrumentID: typeVal },
				success: function (data) {
					$("#materialContainer").html(data);
					materialComponents();
				}
			})

		});
	};

	instrumentChange();

	// Filter list by type
	var typeChange = function () {
		$("#Type").change(function () {
			var typeVal = $(this).val();
			var name = $("#materialContainer").data("name");
			var url = "/Home/_" + name + "Container";

			$.ajax({
				type: "GET",
				url: url,
				dataType: "text",
				data: { materialTypeID: typeVal },
				success: function (data) {
					$("#materialContainer").html(data);
					materialComponents();
				}
			})
		});
	};

	typeChange();

	// Find Materials
	var materialComponents = function () {
		$("#materialAdd").click(function () {
			var typeVal = $("#MaterialID").val();
			var quantity = $("#MaterialQuantity").val();
			var url = "/Home/_MaterialList";

			var idList = [];
			var quantityList = [];

			$(".resources .id").each(function () {
				idList.push($(this).val())
			});

			$(".resources .quantity").each(function () {
				quantityList.push($(this).text())
			});

			$.ajax({
				type: "GET",
				url: url,
				traditional: true,
				dataType: "text",
				data: { val: typeVal, quantity: quantity, idList: idList, quantityList: quantityList },
				success: function (data) {
					$(".resources tbody").html(data);
				},
				error: function (e) {
					//alert(e.statusText)
				}
			})

			$.ajax({
				type: "GET",
				url: "/Home/_ProductList",
				dataType: "text",
				traditional: true,
				data: { val: typeVal, quantity: quantity, idList: idList, quantityList: quantityList },
				success: function (data) {
					$(".products tbody").append(data);
					trash();
					add();
				},
				error: function (e) {
					//alert(e.statusText)
				}
			})
		});
	};

	materialComponents();

	// Remove product and it`s components from list
	var trash = function () {
		$('.trash').click(function () {
			var $this = $(this);
			var id = $this.closest('tr').data('id');
			var quantity = $this.closest('tr').find(".quantity").text();
			var idList = [];
			var quantityList = [];

			$(".resources .id").each(function () {
				idList.push($(this).val())
			});

			$(".resources .quantity").each(function () {
				quantityList.push($(this).text())
			});


			$.ajax({
				type: "GET",
				url: "/Home/_MaterialList",
				dataType: "text",
				traditional: true,
				data: { val: id, quantity: quantity, idList: idList, quantityList: quantityList, isMinus: true },
				success: function (data) {
					$(".resources tbody").html(data);

					quantity -= 1;

					if (quantity == 0) {
						$this.closest('tr').remove();
					} else {
						$this.closest('tr').find(".quantity").text(quantity)
					}
				},
				error: function (e) {
					//alert(e.statusText)
				}
			})
		})
	}

	// Add product and it`s components to list
	var add = function () {
		$('.add').click(function () {
			var $this = $(this);
			var id = $this.closest('tr').data('id');
			var quantity = $this.closest('tr').find(".quantity").text();
			var idList = [];
			var quantityList = [];

			$(".resources .id").each(function () {
				idList.push($(this).val())
			});

			$(".resources .quantity").each(function () {
				quantityList.push($(this).text())
			});

			$.ajax({
				type: "GET",
				url: "/Home/_MaterialList",
				dataType: "text",
				traditional: true,
				data: { val: id, quantity: quantity, idList: idList, quantityList: quantityList, isPlus: true },
				success: function (data) {
					$(".resources tbody").html(data);

					quantity = parseInt(quantity) + 1;
					$this.closest('tr').find(".quantity").text(quantity)
					
				},
				error: function (e) {
					//alert(e.statusText)
				}
			})
		})
	}
});