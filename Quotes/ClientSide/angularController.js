(function () {
    "use strict";

    //-----Controller-------------------------------------------------------------------------
    angular.module(APPNAME).controller('quoteManageController', QuoteManageController);

    QuoteManageController.$inject = ['$scope', '$baseController', '$uibModal', '$quoteService'
        , '$notificationService', '$alertService', '$quoteStateMachineService'];

    function QuoteManageController(
        $scope
        , $baseController
        , $uibModal
        , $quoteService
        , $notificationService
        , $alertService
        , $quoteStateMachineService) {

        var vm = this;
        vm.items = null;
        vm.bidInfo = null;
        vm.convertToQuote = false;
        vm.quoteItemName = null;
        vm.volume = null;
        vm.weight = null;
        vm.quantity = null;
        vm.unit = null;
        vm.quoteId = $("#QUOTEID").val();
        vm.quoteItemsList = null;
        vm.bidId = null;
        vm.userCompanyId = $("#PAGECOMPANY").val();
        vm.userCompanyRole = $("#COMPANYROLE").val();
        vm.quoteState = null;
        vm.thisEventId = null;
        vm.quoteInfo = null;
        vm.sampleUnit =
            [

                    {
                        name: 'Square Feet'
                    },
                    {
                        name: 'Linear Feet'
                    },
                    {
                        name: 'lbs'
                    },
                    {
                        name: 'Count'
                    }
            ];

        vm.$quoteService = $quoteService;
        vm.$notificationService = $notificationService;
        vm.$alertService = $alertService;
        vm.$quoteStateMachineService = $quoteStateMachineService;
        vm.$uibModal = $uibModal;
        vm.$scope = $scope;

        vm.receiveItems = _receiveItems;
        vm.receiveItem = _receiveItem;
        vm.onUserSuccess = _onUserSuccess;
        vm.onUserError = _onUserError;
        vm.displayQuoteForm = _displayQuoteForm;
        vm.insertQuote = _insertQuote;
        vm.insertQuoteItem = _insertQuoteItem;
        vm.deleteQuoteItem = _deleteQuoteItem;
        vm.submitFinalizedQuote = _submitFinalizedQuote;
        //--Functions Specifically For Buyer Company----
        vm.quoteAccepted = _quoteAccepted;
        vm.alterQuote = _alterQuote;
        vm.declineQuote = _declineQuote;
        vm.openContractsEditorModal = _openContractsEditorModal;
        vm.viewContract = _viewContract;
        vm.openPublishedContractModal = _openPublishedContractModal;
        vm.approveContract = _approveContract;


        $baseController.merge(vm, $baseController);

        vm.notify = vm.$quoteService.getNotifier($scope);
        vm.notifyStateMachine = vm.$quoteStateMachineService.getNotifier($scope);

        console.log('vm.quoteId: ', vm.quoteId);
        console.log('vm.userCompanyRole: ', vm.userCompanyRole);

        render(vm.quoteId);

        _getStatusById();


        function _getStatusById() {

            console.log('Grabbing the current Status of the QuoteId Project');

            // Call the service for getting the current Status Id
            vm.$quoteStateMachineService.getStatusByQuoteId(vm.quoteId, _onSMGetStatusSuccess, _onUserError);
        };

        // If the service call for getting the status is successful 
        function _onSMGetStatusSuccess(data) {

            vm.notifyStateMachine(function () {
                console.log('Rendering Success handler!!! Heres the data received: ', data);
                //vm.quoteName = data.item.quoteRequestName;
                vm.stateName = data.item.stateName;
                vm.quoteState = data.item.quoteState;
                console.log('vm.quoteState: ', vm.quoteState);
                console.log('The Current Status is: ', vm.stateName);           
            });

            if (vm.quoteState == 2 && vm.userCompanyRole == 2) {
                vm.$alertService.success('Quote Submitted', 'Success!');          
            }
            else if (vm.quoteState == 7 && vm.userCompanyRole == 2) {
                vm.$alertService.success('Quote Completed', 'Success!');       
            }
            else if (vm.quoteState == 3 && vm.userCompanyRole == 1) {
                vm.$alertService.success('Quote Accepted', 'Success!');
            }
            else if (vm.quoteState == 6 && vm.userCompanyRole == 1) {
                vm.$alertService.info('Quote Sent Back For Alteration', 'Info');
            }
            else if (vm.quoteState == 5 && vm.userCompanyRole == 1) {
                vm.$alertService.info('Quote Declined', 'Declined');
            };


            setTimeout(function () {
                $(window).trigger('resize');
                //console.log('trigger resize running');
            }, 200);

        };


        function _updateStatus(Eventid) {

            console.log('Button Pressed');

            //vm.$quoterequestService.GetByQuoteId(vm.selectedQuoteRequestId, _captureProjectName, _onError);
            console.log('current quoteid: ', vm.quoteId);
            var stateData = {

                "quoteId": vm.quoteId,
                "companyId": vm.userCompanyId,
                "quoteState": vm.quoteState,
                "EventId": Eventid
            };

            // We set the Eventid value so we can use it to properly redirect the user
            vm.thisEventId = Eventid;

            //console.log('stateData.EventId: ', stateData.EventId);

            //- update to the appropriate status
            vm.$quoteStateMachineService.updateStateforQuote(stateData.quoteId, stateData, _onSuccessUpdate, _onUserError);

        };


        //- Success handler for when a status updates
        function _onSuccessUpdate(data) {

            console.log("Status Updated!", data);

            _getStatusById();
        };
		

        function render(quoteId) {
            console.log('angular is running', vm.userCompanyId);

            vm.$quoteService.getQuoteInfoForQuoteReviewByQuoteId(quoteId, vm.receiveItem, vm.onUserError);
        };
		

        function _receiveQuoteItemsList(data) {
            console.log('running _receiveQuoteItemsList: ', data);

            vm.notify(function () {
                vm.quoteItemsList = data.items;
            });
        };


        function _receiveItem(data) {
            vm.notify(function () {
                vm.quoteInfo = data.item;
            });

        };

		
        function _displayQuoteForm() {
            console.log('running _displayQuoteForm');

            vm.convertToQuote = true;
        };

		
        function _insertQuote() {
            console.log('running _insertQuote');

            var insertQuotePayload = {
                "bidId": vm.bidId,
                "buyerCompanyId": vm.bidInfo.bidReceivingCompanyId,
                "sellerCompanyId": vm.bidInfo.bidSubmittingCompanyId,
                "quoteRequestId": vm.bidInfo.qrId,
                "quoteRequestItemUniqueId": vm.bidInfo.qriUniqueId
            };

            console.log('insertQuotePayload: ', insertQuotePayload);

            vm.$quoteService.insertQuote(insertQuotePayload, _insertQuoteSuccess, _onUserError);
        };

		
        function _insertQuoteItem() {
            console.log('running _insertQuoteItem');

            var insertQuoteItemPayload = {
                "quoteId": vm.quoteId,
                "name": vm.quoteItemName,
                "volume": vm.volume,
                "weight": vm.weight,
                "quantity": vm.quantity,
                "unit": vm.unit
            };

            vm.$quoteService.insertQuoteItem(insertQuoteItemPayload, _insertQuoteItemSuccess, _onUserError);

        };

		
        function _deleteQuoteItem(quoteItemId) {
            console.log('running _deleteQuoteItem');

            vm.$quoteService.deleteQuoteItem(quoteItemId, _deleteQuoteItemsSuccess, _onUserError);
        };

		
        function _submitFinalizedQuote(EventId) {
            console.log('running _submitFinalizedQuote');

            _updateStatus(EventId);

            vm.$notificationService.notifyBuyerCompanyOfSubmittedQuote(vm.quoteId, _notificationSuccess, _onUserError);
        };

		
        function _deleteQuoteItemsSuccess() {
            console.log('running _deleteQuoteItemsSuccess');

            vm.$quoteService.getQuoteItemsByQuoteId(vm.quoteId, _receiveQuoteItemsList, _onUserError);
        };

		
        function _insertQuoteSuccess(data) {
            console.log('running _insertQuoteSuccess: ', data);

            setTimeout(function () {
                $(window).trigger('resize');              
            }, 200);

            vm.quoteId = data.item;
            vm.quoteState = 1;
        };
		

        function _insertQuoteItemSuccess(data) {
            console.log('running _insertQuoteItemSuccess: ', data);

            vm.$alertService.success('Quote Item Added', 'Success!');           

            vm.quoteItemName = null;
            vm.volume = null;
            vm.weight = null;
            vm.quantity = null;
            vm.unit = null;
            vm.$quoteService.getQuoteItemsByQuoteId(vm.quoteId, _receiveQuoteItemsList, _onUserError);
        };

		
        function _onUserSuccess(data) {
            console.log('success: ', data);
        };

		
        function _notificationSuccess(data) {
            console.log('notificationSuccess: ', data);
        };

		
        function _onUserError(jqXhr, error) {
            console.error(jqXhr);
        };


        function _approveContract(EventId) {
            console.log('running _approveContract');

            _updateStatus(EventId);

            vm.$notificationService.notifyBuyerCompanyOfApprovedContract(vm.quoteId, _notificationSuccess, _onUserError);
        };


        //---------Functions Used For Buyer Company View-------------------------




        //- Functions for buttons other than contract--------------
        function _quoteAccepted(EventId) {
            console.log('running _quoteAcceptedWithoutContract');

            _updateStatus(EventId);

            vm.$notificationService.notifySellerCompanyOfAcceptedQuote(vm.quoteId, _notificationSuccess, _onUserError);
        };

        function _alterQuote(EventId) {
            console.log('running _alterQuote');

            _updateStatus(EventId);
        };

        function _declineQuote(EventId) {
            console.log('running _declineQuote');

            _updateStatus(EventId);
        };

        //------------------------------------------------------


        function _openContractsEditorModal() {

            var contractsEditorModal = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Scripts/app/Contracts/Templates/editorTemplate.html',
                controller: 'contractsEditorModalController as cemc',
                size: 'lg',
                resolve: {
                    items: function () {

                    }
                }
            });

            contractsEditorModal.result.then(function (data) {

                console.log('Contract URL:', data);

                vm.publishedContractURL = data;

            }, function () {

            });
        };

		
        function _viewContract() {

            console.log('view contract fired');

            window.open(vm.publishedContractURL);
        };

		
        function _openPublishedContractModal() {

            var publishedContractModal = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Scripts/app/Contracts/Templates/pdfTemplate.html',
                controller: 'publishedContractModalController as pcmc',
                size: 'sm',
                resolve: {
                    publishedContract: function () {
                        var contractUrlAndQuoteState = { contractUrl: vm.publishedContractURL, quoteState: vm.quoteState };

                        return contractUrlAndQuoteState;

                    }
                }
            });

			
            publishedContractModal.result.then(function () {
                _getStatusById();
            }, function () {

            });
        };

        //-----------------------------------------------------------------------

    };

})();