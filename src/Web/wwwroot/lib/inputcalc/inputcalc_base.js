$(function () {
	var $screen = $('#screen');
	var $screenwrapper = $('.screenwrapper');
	var $answer = $('.ans');
	var $keys = $('span');
	var $equals = $('.equals');
	var $message = $('.message');
	var $fakeCursor = $('#fakecursor');

	var offsetLeft = $screen.offset().left;
	//will offset adjust on resize???????
	var mouseDown;
	var startX = 0;
	var newX = 0;
	var diffX = 0;
	var scrollPos = 0;

	var eq = '';
	var num = '';
	var ans;
	var equalsWasJustPressed = false;
	var maxPrecision;
	var maxChars;

	var numOfChars;
	var screenWidth;
	// var expandScreen=false;

	var anOperator = /[/*+-]/;
	var aNumber = /\d+/;
	var floats = /[0-9]*\.?[0-9]+\.?/g;
	var aBracket = /[()]/;
	var lastIndex = eq.length - 1;
	var lastChar = eq.charAt(lastIndex);
	var numArr;

	var allowedKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '(', ')', '/', '*', '-', '+', '=', 'Enter', 'Delete', 'Backspace'];

	var buttonAlias = {
		'Enter': '=',
		'Delete': 'C',
		'Backspace': 'CE'
	};

	String.prototype.replaceLastChar =
		function (index, replacement) {
			return this.substr(0, index) + replacement + this.substr(index + replacement.length);
		};

	function clearAns() {
		$answer.text('');
		equalsWasJustPressed = false;
		numbers = [];
	}

	function setNumOfDigits() {
		if ($(document).width() <= 400) {
			maxChars = 10;
			maxPrecision = 5;
		} else {
			maxChars = 13;
			maxPrecision = 9;
		}
	}

	setNumOfDigits();

	$(window).resize(function () {
		setNumOfDigits();
	});

	function process(key) {
		$message.hide();
		$fakeCursor.show();
		var hideCursor = false;
		// lengthenEqWidth(false);
		// var expandScreen=false;
		/*-------------
			DECIMAL
		-------------*/
		if (key === '.') {
			console.log('pressed: decimal');
			if (equalsWasJustPressed) {
				eq = '0' + key;
				clearAns();
			} else {
				if (lastChar.match(aNumber) && numArr[numArr.length - 1].indexOf('.') < 0) {
					eq += key;
				}
				if (lastChar.match(anOperator) || lastChar == '(' || eq == '') {
					eq += '0' + key;
				}
			}
		}
		/*-------------
			NUMBER
		-------------*/
		if (key.match(aNumber)) {
			console.log('pressed: number');
			if (equalsWasJustPressed) {
				eq = key;
				clearAns();
			} else {
				eq += key;
			}
		}
		/*-------------
			OPERATOR
		-------------*/
		if (key.match(anOperator)) {
			console.log('pressed: operator');
			if (equalsWasJustPressed) {
				if ($answer.text() !== 'SYNTAX ERROR' && $answer.text() !== 'INFINITY' /*|| $answer.text()!='undefined' */) {
					eq = ans + key;
				}
				clearAns();
			} else {
				if (lastChar.match(anOperator) || lastChar == '.') {
					eq = eq.replaceLastChar(lastIndex, key);
				} else if (lastChar != '(') {
					eq += key;
				}
			}
		}
		/*-------------
			CLEAR
		-------------*/
		if (key == 'C') {
			console.log('pressed: clear');
			eq = num;
			clearAns();
		}
		/*-------------
			BACKSPACE
		-------------*/
		if (key == 'CE') {
			console.log('pressed: backspace');
			if (equalsWasJustPressed) {
				$answer.text('');
				equalsWasJustPressed = false;
			} else {
				eq = eq.slice(0, -1);
			}
		}
		/*-------------
			BRACKETS	
		-------------*/
		if (key.match(aBracket)) {
			console.log('pressed: bracket');
			if (equalsWasJustPressed) {
				if (key == '(') {
					eq = key;
				} else {
					eq = '';
				}
				clearAns();
			} else {
				if (key == '(') {
					if (lastChar.match(aNumber) || lastChar == '.' || lastChar == ')') {
						eq += '*' + key;
					}
					else if (eq == '') {
						eq = key;
					} else {
						eq += key;
					}
				} else {
					var numOfOpenBrackets = (eq.match(/\(/g) || []).length;
					var numOfCloseBrackets = (eq.match(/\)/g) || []).length;
					if (numOfCloseBrackets < numOfOpenBrackets) {
						if (lastChar !== '(') {
							eq += key;
						}
						if (lastChar.match(anOperator)) {
							eq = eq.replaceLastChar(lastIndex, key);
						}
					}
					// else if (lastChar.match(anOperator)) {
					// 	eq=eq.replaceLastChar(lastIndex,key);
					// } else {
					// 	eq+=key;
					// }
				}
			}
		}
		/*-------------
			EQUALS	
		-------------*/
		if (key == '=') {
			console.log('pressed: equals');
			// lengthenEqWidth(true);
			// expandScreen=true;
			equalsWasJustPressed = true;
			if (lastChar.match(anOperator) || lastChar == '(') {
				eq = eq.slice(0, -1);
			}
			var openBrackets = (eq.match(/\(/g) || []).length;
			var closeBrackets = (eq.match(/\)/g) || []).length;
			if (openBrackets > closeBrackets) {
				for (i = 0; i < (openBrackets - closeBrackets); i++) {
					eq += ')';
				}
			}
			if (eq !== '') {
				hideCursor = true;
				try {
					eval(eq);
					ans = eval(eq);
					if (ans.toString().length > maxChars) {
						if (ans % 1 != 0) {
							$answer.text(parseFloat(ans).toPrecision(maxPrecision));
						} else {
							$answer.text((ans).toPrecision(maxPrecision));
						}
					} else {
						if (ans.toString() == 'NaN') {
							console.log('ans is NaN');
							$answer.text('syntax error'.toUpperCase());
						} else if (ans.toString() == 'Infinity') {
							$answer.text('infinity'.toUpperCase());
						} else {
							$answer.text(ans);
						}
					}
				} catch (err) {
					console.log('error: ' + err);
					$answer.text('syntax error'.toUpperCase());
				}
			}
		}
		numArr = eq.match(floats);
		lastIndex = eq.length - 1;
		lastChar = eq.charAt(lastIndex);
		$screen.text(eq);
		numOfChars = $screen.text().length;
		screenWidth = (numOfChars * 20) + 23;
		if (numOfChars > 0) {
			$screen.css({
				'width': screenWidth + 'px',
			});
		}
		/*else {
			$screen.css({
				'width': 23+'px',
			});	
		}*/
		// $screen.css({
		// 	'width': ((numOfChars*20)+23)+'px',
		// });
		// lengthenEqWidth(expandScreen);
		if (equalsWasJustPressed) {
			$screenwrapper.scrollLeft($screenwrapper.scrollLeft() - 23);
		} else {
			$screenwrapper.get(0).scrollLeft = $screenwrapper.get(0).scrollWidth;
		}
		moveCursor(numOfChars, hideCursor);
		console.log('eq: ' + eq);
		// console.log('no of (: '+eq.match(/\(/g).length);
		// console.log('no of ): '+eq.match(/\)/g).length);
		if (numArr) {
			console.log('numArr: ' + numArr);
			console.log('decimal index: ' + numArr[numArr.length - 1].indexOf('.'));
		}
		console.log('===============================================');
		console.log('///////////////////////////////////////////////');
		console.log('===============================================');
	}

	// function lengthenEqWidth(bool) {
	// 	if (bool) {
	// 		$screen.css({
	// 			// 'width':screenWidth+'px',
	// 			'width':(screenWidth-23)+'px',
	// 		});
	// 	} 
	// 	else {
	// 		$screen.css({
	// 			'width':(screenWidth-23)+'px',
	// 		});
	// 	}
	// }

	function moveCursor(numOfChars, hideCursor) {
		var shift = numOfChars * 20;
		if (hideCursor) {
			$fakeCursor.css({
				'visibility': 'hidden',
			});
		} else {
			$fakeCursor.css({
				'visibility': 'visible',
			});
			// if (shift>=270) {
			// 	$fakeCursor.css({
			// 		'transform': 'translateX(270px)',
			// 	});
			// } else {
			if (shift >= 0) {
				$fakeCursor.css({
					'transform': 'translateX(' + shift + 'px)',
				});
			}
			// }
		}
	}

	function activateButton(buttonValue, active) {
		if (active) {
			$('span[data-value="' + buttonValue + '"]').addClass('active');
		} else {
			$('span[data-value="' + buttonValue + '"]').removeClass('active');
		}
	}

	function getButtonVal(keyPressed) {
		if (buttonAlias[keyPressed]) {
			return buttonAlias[keyPressed];
		} else {
			return keyPressed;
		}
	}

	$(document).on({
		keydown: function (event) {
			var keyPressed = event.key;
			if (allowedKeys.indexOf(keyPressed) >= 0) {
				var buttonValue = getButtonVal(keyPressed);
				process(buttonValue);
				activateButton(buttonValue, true);
			}
		},
		keyup: function (event) {
			var keyPressed = event.key;
			if (allowedKeys.indexOf(keyPressed) >= 0) {
				var buttonValue = getButtonVal(keyPressed);
				activateButton(buttonValue, false);
			}
		}
	});

	$keys.on('click', function () {
		var keyPressed = $(this).data('value');
		if (typeof keyPressed == 'number') {
			keyPressed = keyPressed.toString();
		}
		process(keyPressed);
	});

	$screenwrapper.on('mousedown', function (downEvent) {
		startX = downEvent.pageX - offsetLeft;
		// startScrollX=$(this).scrollLeft();
		mouseDown = true;
	});

	$(document).on('mousemove', function (moveEvent) {
		if (mouseDown) {
			/*$screen.css({
					'cursor':'pointer',
					'cursor': '-moz-pointer',
					'cursor': '-webkit-pointer',
				});*/
			newX = moveEvent.pageX - offsetLeft;
			diffX = newX - startX;
			scrollPos = $screenwrapper.scrollLeft();

			$screenwrapper.scrollLeft($screenwrapper.scrollLeft() - diffX);

			// $('.startX').html('startX: '+startX);
			// $('.newX').html('newX: '+newX);
			// $('.diffX').html('diffX: '+diffX);
			// $('.scrollPos').html('scrollPos: '+$screen.scrollLeft());
		}
	});

	$(document).on('mouseup', function (upEvent) {
		startX = 0;
		newX = 0;
		diffX = 0;
		scrollPos = 0;
		mouseDown = false;
	});

});