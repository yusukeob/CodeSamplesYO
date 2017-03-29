(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('profilePageNotificationController', ProfilePageNotificationController);

    ProfilePageNotificationController.$inject = ['$scope', '$rootScope', '$baseController', '$notificationService'];

    function ProfilePageNotificationController(
        $scope,
        $rootScope,
        $baseController,
        $notificationService) {

        var vm = this;
        var i = null;
        vm.notifications = null;
        vm.unreadNotifications = null;
        vm.selection = [];
        vm.currentUserId = $('#PAGEUSER').val();

        vm.$notificationService = $notificationService;
        vm.$rootScope = $rootScope;
        vm.$scope = $scope;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$notificationService.getNotifier($scope);
        vm.selectAllCheckBoxes = _selectAllCheckBoxes;
        vm.uncheckAllCheckBoxes = _uncheckAllCheckBoxes;
        vm.deleteCheckedNotifications = _deleteCheckedNotifications;
        vm.markAsReadCheckedNotifications = _markAsReadCheckedNotifications;


        _renderNotifications();

        vm.$scope.$on('renderNotifications', _renderNotifications);


        function _renderNotifications() {
            vm.$notificationService.getNotificationsByUserId(vm.currentUserId, _receiveNotifications, _onUserError);
            vm.$notificationService.getUnreadNotificationsByUserId(vm.currentUserId, _receiveUnreadNotifications, _onUserError);
        };

        function _broadcast() {
            vm.$rootScope.$broadcast('renderNotifications');
        };

        function _receiveNotifications(data) {
            vm.notify(function () {
                vm.notifications = data.items;

                if (vm.notifications !== null) {
                    for (i = 0; i < vm.notifications.length; i++) {
                        vm.notifications[i].selected = false;
                    };
                };
            });
        };

        function _receiveUnreadNotifications(data) {
            vm.notify(function () {
                vm.unreadNotifications = data.items;
            });
        };

        function _deleteCheckedNotifications() {
            for (i = 0; i < vm.notifications.length; i++) {
                if (vm.notifications[i].selected == true) {
                    vm.$notificationService.deleteNotification(vm.notifications[i].notificationId, _onUserSuccess, _onUserError);
                };
            };
        };

        function _markAsReadCheckedNotifications() {
            console.log('running _markAsReadCheckedNotifications');

            for (i = 0; i < vm.notifications.length; i++) {
                if (vm.notifications[i].selected == true) {
                    vm.$notificationService.updateAsReadNotification(vm.notifications[i].notificationId, _onUserSuccess, _onUserError);
                };
            };
        };

        function _selectAllCheckBoxes() {
            console.log('running _selectAllCheckBoxes');

            for (i = 0; i < vm.notifications.length; i++) {
                vm.notifications[i].selected = true;
            };
        };

        function _uncheckAllCheckBoxes() {
            console.log('running _uncheckAllCheckBoxes');

            for (i = 0; i < vm.notifications.length; i++) {
                vm.notifications[i].selected = false;
            };
        };

        function _onUserSuccess(data) {
            console.log('success: ', data);

            _broadcast();
            vm.selection = [];
        };

        function _onUserError(jqXhr, error) {
            console.error(error);
        };
    };
})();