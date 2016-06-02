'use strict';

learniApp.controller('MenuController',
    function MenuController($scope, $rootScope, $modal, $location, usersService) {

        $scope.newPackage = function () {
            var modalInstance = $modal.open({
                templateUrl: 'App/templates/EditPackage.html',
                controller: 'EditPackageController',
                resolve: {
                    packageObject: function () {
                        return null;
                    }
                }
            });

            modalInstance.result.then(function (packageObject) {
                $location.path('package/' + packageObject.id);
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.logout = function () {
            if ($rootScope.userAuthenticated) {
                usersService.logout().then(function () {
                    $rootScope.userAuthenticated = false;
                    $location.path('/login');
                });
            }
        };

    });