$(document).ready(function () {

	$("#Type").change(function () {
		$("#FilterType").val($(this).find(":selected").text());
	})

	var addBox = function () {
		$(".plus").click(function () {
			var $box = $(".box").first().clone();
			var num = $(".box").length;
			$box.find("#ResourceListViewModel_0__ID").attr("name", "ResourceListViewModel[" + num + "].ID")
			$box.find("#ResourceListViewModel_0__ID").attr("id", "ResourceListViewModel_" + num + "__ID")
			$box.find("#ResourceListViewModel_0__Quantity").attr("name", "ResourceListViewModel[" + num + "].Quantity")
			$box.find("#ResourceListViewModel_0__Quantity").attr("id", "ResourceListViewModel_" + num + "__Quantity")
			$box.insertBefore(".save");
			if ($(".minus").hasClass("disabled")) {
				$(".minus").removeClass("disabled");
			}
		});
	}
	var removeBox = function () {
		$(".minus").click(function () {
			if ($(".box").length > 1) {
				$(".box").last().remove();
			}
			if ($(".box").length == 1) {
				$(".minus").addClass("disabled");
			}
		});
	}
	addBox();
	removeBox();

	// Clear ALL products and their components
	$("#clear").click(function () {
		$(".resources tbody").html("");
		$(".products tbody").html("");
	});

	$(document).on('keypress', function (e) {
		if (e.which == 13 && $(".focusable").is(":focus")) {
			$(".focusable:focus").click();
		}
	});

	//Create PDf from HTML...
	var CreatePDFfromHTML = function () {
		// Create a document
		const doc = new PDFDocument;

		// Pipe its output somewhere, like to a file or HTTP response
		// See below for browser usage
		doc.pipe(fs.createWriteStream('output.pdf'));

		// Embed a font, set the font size, and render some text
		doc.font('fonts/PalatinoBold.ttf')
			.fontSize(25)
			.text('Some text with an embedded font!', 100, 100);

		// Add an image, constrain it to a given size, and center it vertically and horizontally
		doc.image('path/to/image.png', {
			fit: [250, 300],
			align: 'center',
			valign: 'center'
		});

		// Add another page
		doc.addPage()
			.fontSize(25)
			.text('Here is some vector graphics...', 100, 100);

		// Add some text with annotations
		doc.addPage()
			.fillColor("blue")
			.text('Here is a link!', 100, 100)
			.underline(100, 100, 160, 27, { color: "#0000FF" })
			.link(100, 100, 160, 27, 'http://google.com/');

		// Finalize PDF file
		doc.end();
	}

	$("#download").click(function () {
		CreatePDFfromHTML();
	})
});