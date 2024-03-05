function loadCalc() {
    var $screen = $('.calc-screen');
    var $answer = $('.calc-ans');
    var enableLog = false;
    var hasOperator = false;

    if ($screen.length == 0) {
        let screenDiv = document.createElement('div');
        screenDiv.className = "calc-screen";
        screenDiv.setAttribute('hidden', true);

        $("#wrapper").append(screenDiv);
        $screen = $('.calc-screen');
    }

    if ($answer.length == 0) {
        let answerDiv = document.createElement('div');
        answerDiv.className = "calc-ans";
        answerDiv.setAttribute('hidden', true);

        $("#wrapper").append(answerDiv);
        $answer = $('.calc-ans');
    }

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
        $answer.val('');
        equalsWasJustPressed = false;
        numbers = [];
    }

    function process(key) {
        //console.log(key);
        // lengthenEqWidth(false);
        // var expandScreen=false;
        /*-------------
            DECIMAL
        -------------*/
        if (key === '.') {
            if (enableLog) console.log('pressed: decimal');
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
            if (enableLog) console.log('pressed: number');
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
            if (enableLog) console.log('pressed: operator');
            if (equalsWasJustPressed) {
                if ($answer.val() !== 'SYNTAX ERROR' && $answer.val() !== 'INFINITY' /*|| $answer.val()!='undefined' */) {
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
            if (enableLog) console.log('pressed: clear');
            eq = num;
            clearAns();
        }
        /*-------------
            BACKSPACE
        -------------*/
        //if (key == 'CE') {
        //	console.log('pressed: backspace');
        //	if (equalsWasJustPressed) {
        //		$answer.val('');
        //		equalsWasJustPressed = false;
        //	} else {
        //		eq = eq.slice(0, -1);
        //	}
        //}
        /*-------------
            BRACKETS
        -------------*/
        if (key.match(aBracket)) {
            if (enableLog) console.log('pressed: bracket');
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
            if (enableLog) console.log('pressed: equals');
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
                hasOperator = false;
                try {
                    eval(eq);
                    ans = eval(eq);
                    if (ans.toString().length > maxChars) {
                        if (ans % 1 != 0) {
                            $answer.val(parseFloat(ans).toPrecision(maxPrecision));
                        } else {
                            $answer.val((ans).toPrecision(maxPrecision));
                        }
                    } else {
                        if (ans.toString() == 'NaN') {
                            if (enableLog) console.log('ans is NaN');
                            $answer.val('syntax error'.toUpperCase());
                        } else if (ans.toString() == 'Infinity') {
                            $answer.val('infinity'.toUpperCase());
                        } else {
                            $answer.val(ans);
                        }
                    }
                } catch (err) {
                    if (enableLog) console.log('error: ' + err);
                    $answer.val('syntax error'.toUpperCase());
                }
            }
        }
        numArr = eq.match(floats);
        lastIndex = eq.length - 1;
        lastChar = eq.charAt(lastIndex);
        $screen.text(eq);
        numOfChars = $screen.text().length;
        screenWidth = (numOfChars * 20) + 23;
        //if (numOfChars > 0) {
        //	$screen.css({
        //		'width': screenWidth + 'px',
        //	});
        //}
        /*else {
            $screen.css({
                'width': 23+'px',
            });
        }*/
        // $screen.css({
        // 	'width': ((numOfChars*20)+23)+'px',
        // });
        // lengthenEqWidth(expandScreen);

        if (enableLog) console.log('eq: ' + eq);
        // console.log('no of (: '+eq.match(/\(/g).length);
        // console.log('no of ): '+eq.match(/\)/g).length);
        if (numArr) {
            if (enableLog) console.log('numArr: ' + numArr);
            if (enableLog) console.log('decimal index: ' + numArr[numArr.length - 1].indexOf('.'));
        }
        if (enableLog) console.log('===============================================');
        if (enableLog) console.log('///////////////////////////////////////////////');
        if (enableLog) console.log('===============================================');
    }

    function getButtonVal(keyPressed) {
        if (buttonAlias[keyPressed]) {
            return buttonAlias[keyPressed];
        } else {
            return keyPressed;
        }
    }

    //$("#tbl_entries").on("keydown", $screen, function (event) {
    //    var asdf = $(this);
    //    var keyPressed = event.key;
    //    if (allowedKeys.indexOf(keyPressed) >= 0) {
    //        var buttonValue = getButtonVal(keyPressed);
    //        process(buttonValue);
    //        asdf.val($answer.val());
    //        console.log(asdf.val());

    //    }
    //});

    $(document).on("keydown", ".decimalInputMask5, .decimalInputMask", function (event) {
        var asdf = $(this);
        var keyPressed = event.key;
        if (allowedKeys.indexOf(keyPressed) >= 0) {
            var buttonValue = getButtonVal(keyPressed);
            process(buttonValue);
            _answer = $answer.val();
            //asdf.val("");
            if (_answer != "") {
                asdf.val(_answer);
                $answer.val("");
                setTimeout(function () {
                    totalSumEntries();
                }, 1000)
            }

            if (buttonValue.match(anOperator))
                hasOperator = true;

            if (hasOperator) {
                asdf.popover('dispose').popover({ title: 'Calc', content: $screen.html(), trigger: "focus" });
                asdf.popover('show');
                asdf.val("0.00");
            } else {
                asdf.popover('dispose');
            }
        }
    });
}