learniApp.controller('PackagesController',
    function PackagesController($scope, categoriesService, packagesService) {

        categoriesService.getCategories().then(function (categories) {
            $scope.categories = categories;
            $scope.currentCategory = categories[0];

            packagesService.getPackagesByCategory($scope.currentCategory).then(function (packages) {
                $scope.packages = packages;
            });
        });

        $scope.goToCategory = function (category) {
            $scope.currentCategory = category;

            packagesService.getPackagesByCategory($scope.currentCategory).then(function (packages) {
                $scope.packages = packages;
            });
        };

    });