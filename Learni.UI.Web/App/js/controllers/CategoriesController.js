'use strict';

learniApp.controller('CategoriesController',
    function CategoriesController($scope, categoriesService) {
        categoriesService.getCategories().then(function(categories) {
            $scope.categories = categories;
        });

        $scope.goToCategory = function(category) {
            console.log(category);
        };

    });