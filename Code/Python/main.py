#!/usr/bin/python
#coding=utf-8
import sys
import requests
import util
import os
import random
import getpass
import samples

def log_in(base_url, username, password): 	
        """login and get a access token
        
        Arguments:
                base_url {string} -- authrization base url(currently same with api)
                username {[type]} -- your username
                password {[type]} -- your password
        
        Returns:
                json -- token entity include expiration and refresh token info like:
                        {
                                "access_token": "ABCD1234",      # Access permission
                                "token_type": "bearer",		 # Token type
                                "expires_in": 86399,		 # Access Token Expiration time (in seconds)(It is recommended to use the same token repeatedly within this time frame.) 
                                "refresh_token": "refresh_token" # To refresh Access Token
                        }
        """
        print('Get token:')
        content = 'username={0}&password={1}&grant_type=password'.format(username, password)
        token_entity = requests.post(base_url + 'token', data = content).json()

        if 'access_token' in token_entity:
                print(token_entity)
                return token_entity
        else:
                print(token_entity['error_description'])
                os._exit(-2)

def start_test(base_url, token_entity):
        """Start the samples test
        
        Arguments:
                base_url {string} -- api base url, like http://advancedapi.octoparse.com/ (for China: http://advancedapi.bazhuayu.com/)
                token_entity {json} -- token entity after logged in
        """
        samples.refresh_token(base_url, token_entity['refresh_token'])

        token = token_entity['access_token']

        ###=================bellow are basic apis: http://dataapi.octoparse.com/ (for China: http://dataapi.bazhuayu.com/ )====================###
        groups = samples.get_task_group(base_url, token)
        tasks = samples.get_task_by_group_id(base_url, token, groups[1]['taskGroupId'])

        task_id = tasks[0]['taskId']
        samples.get_data_by_offset(base_url, token, task_id)

        samples.export_not_exported_data(base_url, token, task_id)

        samples.mark_data_as_exported(base_url, token, task_id)

        ###=================bellow are advanced apis: http://advancedapi.octoparse.com/ (for China: http://advancedapi.bazhuayu.com/ )====================###
        task_id = tasks[1]['taskId'] # bellow methods will update your task and clear your task data

        action_name = 'navigateAction1.Url' # make sure the task has this action
        property_value = 'https://www.octoparse.com/features-tutorial?pageIndex=' + str(random.randint(1,5))
        samples.update_task_rule_property(base_url, token, task_id, action_name, property_value)
        samples.get_task_rule_property(base_url, token, task_id, action_name)

        action_name = 'loopAction2.TextList' # make sure the task has this action
        property_value = '["bazhuayu", "skieer"]'
        samples.update_task_rule_property(base_url, token, task_id, action_name, property_value)
        samples.get_task_rule_property(base_url, token, task_id, action_name)
        property_value = '["octoparse"]'
        samples.add_url_or_text_item(base_url, token, task_id, action_name, property_value)
        samples.get_task_rule_property(base_url, token, task_id, action_name)

        samples.start_task(base_url, token, task_id)
        
        samples.get_tasks_status(base_url, token, task_id)

        samples.stop_task(base_url, token, task_id)

        samples.get_tasks_status(base_url, token, task_id)

        samples.remove_task_data(base_url, token, task_id)


if __name__ == '__main__':
        if len(sys.argv) < 2:
                print('Please sepecify your username!')
                os._exit(-1)

        user_name = sys.argv[1]
        password = getpass.getpass('Password:')
        
        base_url = 'http://advancedapi.octoparse.com/'
        token_entity = log_in(base_url, user_name, password)

        start_test(base_url, token_entity)
# End