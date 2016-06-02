learniApp.factory('usersService', function (Restangular) {
    return {
        login: function (user) {
            return Restangular.all('login').post(user);
        },

        logout: function () {
            return Restangular.all('logout').getList();
        },

        register: function (user) {
            return Restangular.all('register').post(user);
        }
    };
});