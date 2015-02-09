//
//  VungleManager.m
//  VungleTest
//
//  Created by Mike Desaro on 6/5/12.
//  Copyright (c) 2012 prime31. All rights reserved.
//

#import "VungleManager.h"
#import <objc/runtime.h>


#if UNITY_VERSION < 500
void UnityPause( bool pause );
#else
void UnityPause( int pause );
#endif

UIViewController *UnityGetGLViewController();

void UnitySendMessage( const char * className, const char * methodName, const char * param );


@implementation VungleManager

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Class Methods

+ (VungleManager*)sharedManager
{
	static VungleManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[VungleManager alloc] init];
	
	return sharedSingleton;
}


+ (id)objectFromJson:(NSString*)json
{
	NSError *error = nil;
	NSData *data = [NSData dataWithBytes:json.UTF8String length:json.length];
    NSObject *object = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingAllowFragments error:&error];
	
	if( error )
		NSLog( @"failed to deserialize JSON: %@ with error: %@", json, [error localizedDescription] );

    return object;
}


+ (NSString*)jsonFromObject:(id)object
{
	NSError *error = nil;
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:0 error:&error];
	
	if( jsonData && !error )
		return [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
	else
		NSLog( @"jsonData was null, error: %@", [error localizedDescription] );

    return @"{}";
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Public

- (void)startWithAppId:(NSString*)appId
{
	[VungleSDK sharedSDK].delegate = self;
	[[VungleSDK sharedSDK] startWithAppId:appId];
}


- (void)playAdWithOptions:(NSDictionary*)options
{
	[[VungleSDK sharedSDK] playAd:UnityGetGLViewController() withOptions:options error:NULL];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - VGVunglePubDelegate

- (void)vungleSDKwillShowAd
{
	UnitySendMessage( "VungleManager", "vungleSDKwillShowAd", "" );
}


- (void)vungleSDKwillCloseAdWithViewInfo:(NSDictionary*)viewInfo willPresentProductSheet:(BOOL)willPresentProductSheet
{
	if( !willPresentProductSheet )
		UnityPause( false );
	
	BOOL completedView = [[viewInfo objectForKey:@"completedView"] boolValue];
	BOOL didDownlaod = [[viewInfo objectForKey:@"didDownlaod"] boolValue];
	
	NSDictionary *dict = @{
						   @"completedView": [NSNumber numberWithBool:completedView],
						   @"playTime": [viewInfo objectForKey:@"playTime"],
						   @"didDownload": [NSNumber numberWithBool:didDownlaod],
						   @"willPresentProductSheet": [NSNumber numberWithBool:willPresentProductSheet]
						   };
	UnitySendMessage( "VungleManager", "vungleSDKwillCloseAd", [VungleManager jsonFromObject:dict].UTF8String );
}


- (void)vungleSDKwillCloseProductSheet:(id)productSheet
{
	UnityPause( false );
	UnitySendMessage( "VungleManager", "vungleSDKwillCloseProductSheet", "" );
}


- (void)vungleSDKhasCachedAdAvailable
{
	UnitySendMessage( "VungleManager", "vungleSDKhasCachedAdAvailable", "" );
}

@end
