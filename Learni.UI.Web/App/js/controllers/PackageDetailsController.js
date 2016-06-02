learniApp.controller('PackageDetailsController',
    function PackageDetailsController($scope, $routeParams, $location, $modal, packagesService, termsService) {
        packagesService.getPackageById($routeParams.packageId).then(function (packageObject) {
            $scope.package = packageObject;

            termsService.getTermsByPackage(packageObject).then(function (terms) {
                $scope.terms = terms;
            });
        });

        $scope.addTerm = function () {
            var modalInstance = $modal.open({
                templateUrl: 'App/templates/EditTerm.html',
                controller: 'EditTermController',
                resolve: {
                    packageObject: function () {
                        return $scope.package;
                    },
                    term: function () {
                        return null;
                    }
                }
            });

            modalInstance.result.then(function (term) {
                $scope.terms.push(term);
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.deleteTerm = function(term) {
            termsService.deleteTerm(term.id).then(function (deletedTermId) {
                var index = $scope.terms.indexOf(term);
                if (index > -1) {
                    $scope.terms.splice(index, 1);
                }
            });
        };

        $scope.deletePackage = function(packageObject) {
            packagesService.deletePackage(packageObject.id).then(function (deletedPackageId) {
                $location.path('packages');
            });
        };

        $scope.editPackage = function(packageObject) {
            var modalInstance = $modal.open({
                templateUrl: 'App/templates/EditPackage.html',
                controller: 'EditPackageController',
                resolve: {
                    packageObject: function () {
                        return angular.copy(packageObject);
                    }
                }
            });

            modalInstance.result.then(function (editedPackage) {
                $scope.package = editedPackage;
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.editTerm = function(term) {
            var modalInstance = $modal.open({
                templateUrl: 'App/templates/EditTerm.html',
                controller: 'EditTermController',
                resolve: {
                    packageObject: function () {
                        return $scope.package;
                    },
                    term: function() {
                        return angular.copy(term);
                    }
                }
            });

            modalInstance.result.then(function (editedTerm) {
                var index = $scope.terms.indexOf(term);
                $scope.terms[index] = editedTerm;
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };
    });