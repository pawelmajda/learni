'use strict';

learniApp.directive('ngConfirmClick', [
        function() {
            return {
                link: function(scope, element, attr) {
                    var msg = attr.ngConfirmClick || "Are you sure?";
                    var clickAction = attr.confirmedClick;
                    element.bind('click', function(event) {
                        if (window.confirm(msg)) {
                            scope.$eval(clickAction);
                        }
                    });
                }
            };
        }
]);

learniApp.directive('pwCheck', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            scope.$watch(attrs.pwCheck, function (confirmPassword) {
                var isValid = ctrl.$viewValue === confirmPassword;
                ctrl.$setValidity('pwmatch', isValid);
            });
        }
    }
})

learniApp.directive('ngMatch', [
    function () {
	    return {
	        require: 'ngModel',
	        link: function (scope, elem, attrs, ctrl) {
	            var me = attrs.ngModel;
	            var matchTo = attrs.ngMatch;
	            scope.$watch('[me, matchTo]', function (value) {
	                ctrl.$setValidity('ngmatch', scope[me] === scope[matchTo]);
	            });
	        }
	    }
    }
]);
