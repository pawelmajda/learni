learniApp.factory('termsService', function (Restangular) {
    return {
        getTermsByPackage: function (packageObject) {
            return packageObject.getList('terms');
        },

        getTermById: function (termId) {
            return Restangular.one('terms', termId).get();
        },

        saveTerm: function (term) {
            return Restangular.all('terms').post(term);
        },

        deleteTerm: function (termId) {
            return Restangular.one('terms', termId).remove();
        }
    };
});