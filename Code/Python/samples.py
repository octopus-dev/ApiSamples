#!/usr/bin/python
#coding=utf-8
import sys
import requests
import time
import util
import os
import random

def refresh_token(base_url, refresh_token_id):
        """refresh the token with refresh token id
        
        Arguments:
                base_url {string} -- base url of the api
                refresh_token_id {string} -- a refresh token id from a token entity
        
        Returns:
                string -- a refreshed token string
        """
        print('Refresh token with: ' + refresh_token_id)
        content = 'refresh_token=' + refresh_token_id + '&grant_type=refresh_token'
        response = requests.post(base_url + 'token', data = content)
        token_entity = response.json()
        refresh_token = token_entity.get('access_token',token_entity)
        print(refresh_token)
        return refresh_token

def get_data_by_offset(base_url, token, task_id, offset=0, size=10):
        """Get task data by data offset

        To get data, parameters such as offset, size and task ID are all required in the request. 
        Offset should default to 0 (offset=0), and size∈[1,1000] for making the initial request. 
        The offset returned (could be any value greater than 0) should be used for making the next request. 
        For example, if a task has 1000 data rows, using parameter: offset = 0, size = 100 will return 
        the first 100 rows of data and the offset X (X can be any random number greater than or equal to 100). 
        When making the second request, user should use the offset returned from the first request, offset = X, size = 100 
        to get the next 100 rows of data (row 101 to 200) as well as the new offset to use for the request follows.
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
        
        Keyword Arguments:
                offset {int} -- an offset from last data request, should remains 0 if is the first request (default: {0})
                size {int} -- data row size for the request (default: {10})
        
        Returns:
                json -- task dataList and relevant information:
                         {
                                "data": {
                                "offset": 4,
                                "total": 100000,
                                "restTotal": 99996,
                                "dataList": [
                                {
                                        "state": "Texas",
                                        "city": "Plano"
                                },
                                {
                                        "state": "Texas",
                                        "city": "Houston"
                                },
                                ...
                                ]
                                },
                                "error": "success",
                                "error_Description": "Action Success"
                        }
        """
        print('GetTaskDataByOffset:')
        url = 'api/allData/getDataOfTaskByOffset?taskId=%s&offset=%s&size=%s'%(task_id, offset, size)
        task_data_result = util.request_t_get(base_url, url, token)
        util.show_task_data(task_data_result)
        return task_data_result

def remove_task_data(base_url, token, task_id):
        """Clear data of a task
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
        
        Returns:
                string -- remind message(include error if exists)
        """
        print('RemoveTaskData:')
        url = 'api/task/removeDataByTaskId?taskId=' + task_id
        response = util.request_t_post(base_url, url, token)
        print(response['error_Description'])
        return response

def get_task_group(base_url, token):
        """List All Task Groups
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
        
        Returns:
                list -- all task groups in yout account
        """
        print('GetTaskGroups:')
        url = 'api/taskgroup'
        response = util.request_t_get(base_url, url, token)
        groups = []
        if 'error' in response:
                if response['error'] == 'success':
                        groups = response['data']
                        for taskgroup in groups:
                                print('%s\t%s'%(taskgroup['taskGroupId'],taskgroup['taskGroupName']))
                else:
                        print(response['error_Description'])
        else:
                print(response)
                
        return groups

def get_task_by_group_id(base_url, token, groupId):
        """List All Tasks in a Group
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                groupId {int} -- a task group id
        
        Returns:
                list -- all tasks in a group
        """
        print('GetTasks:')
        url = 'api/task?taskgroupId=' + str(groupId)
        response = util.request_t_get(base_url, url, token)
        if 'error' in response:
                if response['error'] == 'success':
                        tasks = response['data']
                        for task in tasks:
                                print('%s\t%s'%(task['taskId'],task['taskName']))
                else:
                        print(response['error_Description'])
        else:
                print(response)

        return tasks

def export_not_exported_data(base_url, token, task_id):
        """Export Non-exported Data
        
        This returns non-exported data. Data will be tagged status = exporting (instead of status=exported) after the export. 
        This way, the same set of data can be exported multiple times using this method. 
        If the user has confirmed receipt of the data and wish to update data status to ‘exported’, 
        please make a status update.

        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
        
        Returns:
                json -- task dataList and relevant information:
                         {
                                "data": {
                                "offset": 4,
                                "total": 100000,
                                "restTotal": 99996,
                                "dataList": [
                                {
                                        "state": "Texas",
                                        "city": "Plano"
                                },
                                {
                                        "state": "Texas",
                                        "city": "Houston"
                                },
                                ...
                                ]
                                },
                                "error": "success",
                                "error_Description": "Action Success"
                        }
        """
        print('ExportNotExportedData')
        url = 'api/notExportData/getTop?taskId=' + task_id + '&size=10'
        task_data_result = util.request_t_get(base_url, url, token)
        util.show_task_data(task_data_result)
        return task_data_result    

def mark_data_as_exported(base_url, token, task_id):
        """Update Data Status
        
        This updates data status from ‘exporting’ to ‘exported’.
        Note: Please confirm data exported via the API ‘Export Task Data’ (api/notexportdata/gettop) have been retrieved successfully before using this method.

        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
        
        Returns:
                string -- remind message(include error if exists)
        """
        print('MarkDataExported:')
        url = 'api/notExportData/Update?taskId=' + task_id
        response = util.request_t_post(base_url, url, token)
        print(response['error_Description'])
        return response

###=================bellow are advanced apis====================###

def start_task(base_url, token, task_id):
        """Start Running Task
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
        
        Returns:
                string -- remind message(include error if exists)
        """
        print('StartTask:')
        url = 'api/task/startTask?taskId=' + task_id
        response = util.request_t_post(base_url, url, token)
        print(response['error_Description'])
        return response
        
def get_tasks_status(base_url, token, task_id_List):
        """This returns status of multiple tasks.
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string list} -- task id(s) of one or more task(s) from our platform
        
        Returns:
                list -- status list of given task(s)
        """
        print('TaskStatus:')
        url = 'api/task/getTaskStatusByIdList'
        content = { "taskIdList": task_id_List }
        response = util.request_t_post(base_url, url, token, content)
        task_status_list = []
        if 'error' in response:
                if response['error'] == 'success':
                        task_status_list = response['data']
                        for ts in task_status_list:
                                print('%s\t%s\t%s'%(ts['taskId'], ts['taskName'], ts['status']))
                else:
                        print(response['error_Description'])
        else:
                print(response)

        return task_status_list

def stop_task(base_url, token, task_id):
        """Stop Running Task
        
        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform

        Returns:
                string -- remind message(include error if exists)
        """
        print('StopTask:')
        url = 'api/task/stopTask?taskId=' + task_id
        response = util.request_t_post(base_url, url, token)
        print(response['error_Description'])
        return response;

def update_task_rule_property(base_url, token, task_id, action_name, property_value):
        """Update Task Parameters
        
        Use this method to update task parameters (currently only available to updating URL in ‘Go To The Web Page’ action, 
        text value in ‘Enter Text’ action, and text list/URL list in ‘Loop Item’ action).

        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
                action_name {[type]} -- a unique combination name of an action and a parameter from a task, like 'navigateAction1.Url' or 'loopAction2.TextList'
                property_value {[type]} -- value of the parameter to set
        
        Returns:
                string -- remind message(include error if exists)
        """
        print('UpdateTaskRuleProperty: ' + action_name) 
        url = 'api/task/updateTaskRule'
        content = { 'taskId': task_id, 'name': action_name, 'value': json.dumps(property_value) }
        response = util.request_t_post(base_url, url, token, content)
        print(response['error_Description'])
        return response;

def get_task_rule_property(base_url, token, task_id, action_name):
        """Get Task Parameters
        
        This returns the different parameters for a specific task, for example, 
        the URL from ‘Go To The Web Page’ action, text value from ‘Enter Text’ action and text list/URL list from ‘Loop Item’ action.

        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
                action_name {[type]} -- a unique combination name of an action and a parameter from a task, like 'navigateAction1.Url' or 'loopAction2.TextList'
        
        Returns:
                list -- value list of a given action parameter
        """
        print('GetTaskRulePropertyByName: ' + action_name)
        url = 'api/task/GetTaskRulePropertyByName?taskId=' + task_id + '&name=' + action_name
        response = util.request_t_post(base_url, url, token)
        property_list = []
        if 'error' in response:
                if response['error'] == 'success':
                        property_list = response['data']
                        for pro in property_list:
                                print(pro)
                else:
                        print(response['error_Description'])
        else:
                print(response)

        return property_list;


def add_url_or_text_item(base_url, token, task_id, action_name, property_value):
        """Adding URL/Text to a Loop
        
        Use this method to add new URLs/text to an existing loop.

        Arguments:
                base_url {string} -- base url of the api
                token {string} -- token string from a valid token entity
                task_id {string} -- task id of a task from our platform
                action_name {[type]} -- a unique combination name of an action and a parameter from a task, like 'navigateAction1.Url' or 'loopAction2.TextList'
                property_value {[type]} -- value of the parameter to set
        
        Returns:
                string -- remind message(include error if exists)
        """
        print('AddUrlOrTextToTask: ' + action_name) 
        url = 'api/task/AddUrlOrTextToTask'
        content = { 'taskId': task_id, 'name': action_name, 'value': property_value }
        response = util.request_t_post(base_url, url, token, content)
        print(response['error_Description'])
        return response;