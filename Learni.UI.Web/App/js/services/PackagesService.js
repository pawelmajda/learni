learniApp.factory('packagesService', function (Restangular) {
    return {
        getPackagesByCategory: function (category) {
            return category.getList('packages');
        },

        getPackageById: function(packageId) {
            return Restangular.one('packages', packageId).get();
        },

        savePackage: function (packageObject) {
            return Restangular.all('packages').post(packageObject);
        },

        deletePackage: function (packageId) {
            return Restangular.one('packages', packageId).remove();
        }
    };
});