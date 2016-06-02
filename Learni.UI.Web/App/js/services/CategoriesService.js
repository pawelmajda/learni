learniApp.factory('categoriesService', function (Restangular) {
    return {
        getCategories: function () {
            return Restangular.all('categories').getList();
        }
    };
});