(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('authenticatedLayoutNotificationController', AuthenticatedLayoutNotificationController);

    AuthenticatedLayoutNotificationController.$inject = ['$scope', '$rootScope', '$baseController', '$notificationService', '$alertService', '$signalRService'];

    function AuthenticatedLayoutNotificationController(
        $scope,
        $rootScope,
        $baseController,
        $notificationService,
        $alertService,
        $signalRService
        ) {

        var vm = this;
        var i = null;
        vm.unreadNotifications = null;
        vm.alertUnreadNotifications = null;
        vm.currentUserId = $('#PAGEUSER').val();

        vm.$notificationService = $notificationService;
        vm.$signalRService = $signalRService;
        vm.$alertService = $alertService;
        vm.$rootScope = $rootScope;
        vm.$scope = $scope;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$notificationService.getNotifier($scope);

        _renderUnreadNotifications();

        vm.$scope.$on('renderNotifications', _renderUnreadNotifications);

        vm.$signalRService.CallbackRegistration('addNewNotificationToPage', function (message) {
            vm.$alertService.success(message, "New Notification");

            _broadcast();

            console.log("AuthenticatedLayoutNotificationController signalR registered.");

        })


        function _checkForNewNotification() {
            vm.$notificationService.getNotificationsByTimeAndUserId(vm.currentUserId, _alertUnreadNotifications, _onUserError);
        };


        function _alertUnreadNotifications(data) {
            vm.alertUnreadNotifications = data.items;

            if (vm.alertUnreadNotifications != null) {
                vm.$alertService.info('Check your notificatons...', 'New Notification');
                _broadcast();
            };

        };

        function _renderUnreadNotifications() {
            vm.$notificationService.getUnreadNotificationsByUserId(vm.currentUserId, _receiveUnreadNotifications, _onUserError);
        };

        function _broadcast() {
            vm.$rootScope.$broadcast('renderNotifications');
        };


        function _receiveUnreadNotifications(data) {

            vm.notify(function () {
                vm.unreadNotifications = data.items;
            });
        };

        function _onUserSuccess(data) {
            _broadcast();
            vm.selection = [];
        };

        function _onUserError(jqXhr, error) {
            console.error(jqXhr);
        };


    };

})();