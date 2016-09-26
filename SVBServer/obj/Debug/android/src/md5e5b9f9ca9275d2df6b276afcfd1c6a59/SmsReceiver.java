package md5e5b9f9ca9275d2df6b276afcfd1c6a59;


public class SmsReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("SVBServer.Sms.SmsReceiver, SVBServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SmsReceiver.class, __md_methods);
	}


	public SmsReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SmsReceiver.class)
			mono.android.TypeManager.Activate ("SVBServer.Sms.SmsReceiver, SVBServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
