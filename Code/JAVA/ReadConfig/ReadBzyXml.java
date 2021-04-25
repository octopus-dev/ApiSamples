package ReadConfig;

import org.apache.commons.configuration.Configuration;
import org.apache.commons.configuration.ConfigurationException;
import org.apache.commons.configuration.XMLConfiguration;
public class ReadBzyXml {
	
	public Configuration getConfig() throws ConfigurationException{
		Configuration configXml=new XMLConfiguration("XmlConfig/BzyXmlConfig.xml");
		return configXml;
	}
	
	public String getKeyName(String keyName) throws ConfigurationException {
		Configuration configXml=new XMLConfiguration("XmlConfig/BzyXmlConfig.xml");
		// config.getKeys().
		String keyValue = configXml.getString(keyName);
		// System.out.println(keyValue);
		return keyValue;
	}
}
