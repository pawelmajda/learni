'use strict';

learniApp.controller('EditPackageController',
    function EditPackageController($scope, $modalInstance, categoriesService, packagesService, packageObject) {
        categoriesService.getCategories().then(function (categories) {
            $scope.categories = categories;

            if (packageObject) {
                $scope.inEditMode = true;
                $scope.package = packageObject;
            } else {
                $scope.inEditMode = false;
                $scope.package = { categoryId: categories[0].Id };
            }
        });

        $scope.savePackage = function (packageObject, packageForm) {
            if (packageForm.$valid) {
                packagesService.savePackage(packageObject).then(function (savedPackage) { 
                    $modalInstance.close(savedPackage);
                }, function () {
                    console.log("There was an error saving.");
                }); 
            } 
        };

        $scope.cancelEdit = function() {
            $modalInstance.dismiss('cancel');
        };
    }
);